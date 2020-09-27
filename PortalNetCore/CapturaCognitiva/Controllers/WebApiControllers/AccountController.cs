using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Data;
using CapturaCognitiva.Data.Entities;
using CapturaCognitiva.Models.Response;
using CapturaCognitiva.Models.ViewModelsApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapturaCognitiva.Controllers.WebApiControllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : WebApiController
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment env)
        {
            _env = env;
            _db = context;
            _signInManager = signInManager;

        }

        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginApiViewModels model, [FromHeader] string Token)
        {
            var response = new ResponseLogin();
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (!ValidarTokenWeb(Token))
                {
                    return BadRequest(response.SetResponseLogin(-1, false, "Acceso denegado token invalido"));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(response.SetResponseLogin(-2, false, "Informacion invalida", null, GetErroresModelo(ModelState)));
                }
                var User = _db.ApplicationUsers.FirstOrDefault(c => c.Email == model.Email);
                if (User != null)
                {
                    var roles = SessionData.GetNameRole(_db, User.Id);
                    if (roles != "OperadorMobile")
                    {
                        return BadRequest(response.SetResponseLogin(-3, false, "Rol invalido"));
                    }
                    if (User.LockoutEnabled)
                    {
                        return Ok(response.SetResponseLogin(-4, false, "Se ha bloqueado tu usuario"));
                    }
                    if (User.IsRemoved)
                    {
                        return BadRequest(response.SetResponseLogin(-8, false, $"El usuario no existe"));
                    }
                    if (!User.IsEnabled)
                    {
                        return Ok(response.SetResponseLogin(-5, false, "Usuario inhabilitado."));
                    }
                    var result = await _signInManager.PasswordSignInAsync(User.UserName, model.Contraseña, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return Ok(response.SetResponseLogin(1, true, "Exitoso", null, User.Nombres, User.Id));
                    }
                    if (result.IsLockedOut)
                    {
                        return Ok(response.SetResponseLogin(-3, false, "Se ha bloqueado tu usuario"));
                    }
                    else
                    {
                        return BadRequest(response.SetResponseLogin(-3, false, "Informacion invalida"));
                    }
                }
                return BadRequest(response.SetResponseLogin(-8, false, $"El usuario no existe"));

            }
            catch
            {
                scope.Dispose();
                return StatusCode(500, response.SetResponseLogin(-9, false, "Ocurrio un error, comuniquese con el administrador"));
                throw;
            }

        }

        [Route("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordApiViewModels model, [FromHeader] string Token)
        {
            var response = new ResponseRecoveryPassword();
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TransactionManager.MaximumTimeout }, TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (!ValidarTokenWeb(Token))
                {
                    return BadRequest(response.SetResponseRecoveryPassword(-1, false, "Acceso denegado token invalido"));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(response.SetResponseRecoveryPassword(-2, false, "Informacion invalida", GetErroresModelo(ModelState)));
                }
                var userForgot = _db.ApplicationUsers.FirstOrDefault(c => c.Email == model.Email);
                if (userForgot == null)
                {
                    ModelState.AddModelError("", "Informacion invalida.");
                }
                var codigoForgotPassword = _db.CodigoForgotPasswords.FirstOrDefault(c => c.ApplicationUser.Email == model.Email && !c.IsUsed);
                if (codigoForgotPassword == null)
                {
                    CodigoForgotPassword codigoUserForgot = new CodigoForgotPassword
                    {
                        ApplicationUserId = userForgot.Id,
                        Code = Encryptor.GeneratePassword(),
                        FechaCreacion = DateTime.Now,
                        FechaUso = null,
                        IsUsed = false
                    };
                    _db.Add(codigoUserForgot);
                    _db.SaveChanges();
                    EmailHelper emailHelper = new EmailHelper(_env);
                    var apiKey = _db.Roles.FirstOrDefault(c => c.Id == "2").Name;
                    if (await emailHelper.SendPasswordRecovery(userForgot.Nombres, userForgot.Email, codigoUserForgot.Code, apiKey))
                    {
                        return Ok(response.SetResponseRecoveryPassword(1, true, "Revise su correo por favor"));
                    }
                    else
                    {
                        return Ok(response.SetResponseRecoveryPassword(-3, false, "Reintente nuevamente"));
                    }
                }
                else
                {
                    EmailHelper emailHelper = new EmailHelper(_env);
                    var apiKey = _db.Roles.FirstOrDefault(c => c.Id == "2").Name;
                    if (await emailHelper.SendPasswordRecovery(userForgot.Nombres, userForgot.Email, codigoForgotPassword.Code, apiKey))
                    {
                        return Ok(response.SetResponseRecoveryPassword(1, true, "Revise su correo por favor"));
                    }
                    else
                    {
                        return Ok(response.SetResponseRecoveryPassword(-3, false, "Reintente nuevamente"));
                    }
                }
            }
            catch
            {
                scope.Dispose();
                return StatusCode(500, response.SetResponseRecoveryPassword(-9, false, "Ocurrio un error, comuniquese con el administrador"));
                throw;
            }

        }
    }
}
