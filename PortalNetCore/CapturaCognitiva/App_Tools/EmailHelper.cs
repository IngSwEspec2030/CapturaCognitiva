using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Data;
using Microsoft.AspNetCore.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Threading.Tasks;


namespace CapturaCognitiva.Models.Response
{
    public class EmailHelper
    {
        private string ApiKey { get; set; }
        private readonly IWebHostEnvironment _env;
        public EmailHelper(IWebHostEnvironment env)
        {
            _env = env;
        }
        private string BodyRegisterAccount(string nombres, string passgeneric)
        {
            string path = Path.Combine(_env.WebRootPath, "BodyEmails/BodyEmailRecuperacionContraseña.html");
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{NombresCompleto}", nombres);
            body = body.Replace("{Contraseña}", passgeneric);
            return body;
        }


        public async Task<bool> SendPasswordRecovery(string nombres, string emailregister, string code)
        {
            try
            {
                string routeCode = string.Concat(ConfigurationManager.AppSetting["CapturaCogninitvaKeys:UlrHost"],code); 
                string path = Path.Combine(_env.WebRootPath, "BodyEmails/BodyEmailRecuperacionContraseña.html");
                ApiKey = ConfigurationManager.AppSetting["CapturaCogninitvaKeys:App_sendgrid"];
                var client = new SendGridClient(ApiKey);
                var from = new EmailAddress("Notificaciones-noresponse@capturacognitiva.com.co", "Notificaciones");
                var subject = $"Señor : {nombres}";
                var to = new EmailAddress(emailregister, nombres);
                var plainTextContent = "Recuperar contraseña";
                var htmlContent = BodyRegisterAccount(nombres, routeCode);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode.ToString() == "200" || response.StatusCode.ToString() == "Accepted" || response.StatusCode.ToString() == "202")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
                throw;
            }

        }
    }
}