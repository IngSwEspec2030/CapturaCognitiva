using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Data;
using CapturaCognitiva.Models.ViewModelsApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapturaCognitiva.Controllers.WebApiControllers
{
    [Route("api/Cuenta")]
    [ApiController]
    public class AccountController : WebApiController
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment env)
        {
            _env = env;
            _db = context;
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [Route("Registrar")]
        public async Task<ResponseHelper> RegistrarAsync([FromBody] RegisterApIViewModels model, [FromHeader] string Token)
        {
            var response = new ResponseHelper();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ValidarTokenWeb(Token, _db))
                    {
                        return response.SetResponse(-1, false, "Acceso denegado token invalido");
                    }
                    if (!ModelState.IsValid)
                    {
                        return response.SetResponse(-2, false, "Informacion invalida", GetErroresModelo(ModelState));
                    }
                    string telefono = null;
                    if (!string.IsNullOrEmpty(model.Telefono))
                    {
                        telefono = model.Telefono;
                    }
                    var _useremail = _db.ApplicationUsers.FirstOrDefault(c => c.Email == model.Email);
                    if (_useremail != null)
                    {
                        return response.SetResponse(-3, false, "El email ya existe");
                    }
                    var user = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = false,
                        IsEnabled = true,
                        DateEnabled = DateTime.Now,
                        PhoneNumber = telefono,
                        LockoutEnabled = false,
                        Attemps = 0,
                        Nombres = model.Nombres,
                        DateCreated = DateTime.Now,
                    };
                    string _passgeneric = "Colombia123*";
                    var result = await _userManager.CreateAsync(user, _passgeneric);

                    if (!result.Succeeded)
                    {
                        scope.Dispose();
                        return response.SetResponse(-2, false, "Informacion invalida ", result.Succeeded.ToString());
                    }
                    else
                    {                        
                        scope.Complete();
                        return response.SetResponse(1, true, "Usuario creado, verifique su correo :" + model.Email);
                    }
                }
                catch
                {
                    scope.Dispose();
                    return response.SetResponse(-9, false, "Ocurrio un error, comuniquese con el administrador");
                    throw;
                }
            }

        }




    }
}
