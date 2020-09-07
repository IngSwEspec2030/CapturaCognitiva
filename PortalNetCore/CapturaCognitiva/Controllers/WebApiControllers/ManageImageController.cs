using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Data;
using CapturaCognitiva.Data.Entities;
using CapturaCognitiva.Models.Response;
using CapturaCognitiva.Models.ViewModelsApi;
using CapturaCognitiva.WebServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapturaCognitiva.Controllers.WebApiControllers
{
    [Route("api/Image")]
    [ApiController]
    public class ManageImageController : WebApiController
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageImageController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment env)
        {
            _env = env;
            _db = context;
            _signInManager = signInManager;

        }
        [Route("AnalyzerImage")]
        public IActionResult AnalyzerImage([FromBody] AnalyzerImageViewModels model, [FromHeader] string Token)
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
                var User = _db.ApplicationUsers.FirstOrDefault(c => c.Id == model.UserToken);
                if (User != null)
                {
                    WSImage awsImage = new WSImage();
                    var responseAnalyzer = awsImage.AnalyserImage(model.ImageBase64);
                    if (responseAnalyzer.Success)
                    {
                        if (_db.Images.FirstOrDefault(c=> c.Uuid==responseAnalyzer.Uuid)!=null) 
                        {
                            return Ok(response.SetResponseRecoveryPassword(-5, false, "Uuid repetido, comuniquese con el administrador"));
                        }
                        Sender sender = new Sender
                        {
                            State = responseAnalyzer.GuideInfo.Sender.State,
                            Cell = responseAnalyzer.GuideInfo.Sender.Cell,
                            Name = responseAnalyzer.GuideInfo.Sender.Name,
                            Address = responseAnalyzer.GuideInfo.Sender.Address,
                        };
                        _db.Senders.Add(sender);
                        _db.SaveChanges();
                        Receiver receiver = new Receiver
                        {
                            State = responseAnalyzer.GuideInfo.Receiver.State,
                            Cell = responseAnalyzer.GuideInfo.Receiver.Cell,
                            Name = responseAnalyzer.GuideInfo.Receiver.Name,
                            Address = responseAnalyzer.GuideInfo.Receiver.Address,
                        };
                        _db.Receivers.Add(receiver);
                        _db.SaveChanges();
                        Image image = new Image
                        {
                            Uuid = responseAnalyzer.Uuid
                        };
                        _db.Images.Add(image);
                        _db.SaveChanges();
                        Guide guide = new Guide
                        {
                            ImageId = image.Id,
                            IsCompleted = responseAnalyzer.GuideInfo.Complete,
                            ReceiverId = receiver.Id,
                            SenderId = sender.Id
                        };
                        _db.Guides.Add(guide);
                        _db.SaveChanges();
                        scope.Complete();
                        return Ok(response.SetResponseRecoveryPassword(1, true, "exitoso"));
                    }
                    else
                    {
                        return Ok(response.SetResponseRecoveryPassword(-3, false, "ocurrio un error"));
                    }

                }
                return BadRequest(response.SetResponseRecoveryPassword(-8, false, $"Informacion invalida"));

            }
            catch(Exception ex)
            {
                scope.Dispose();
                return StatusCode(500, response.SetResponseRecoveryPassword(-9, false, "Ocurrio un error, comuniquese con el administrador"));
                throw;
            }

        }


    }
}
