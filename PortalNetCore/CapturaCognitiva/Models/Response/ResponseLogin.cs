using CapturaCognitiva.App_Tools;

namespace CapturaCognitiva.Models.Response
{
    public class ResponseLogin : ResponseHelper
    {

        public string UserId { get; set; }
        public string Nombres { get; set; }

        public ResponseLogin()
        {
            Message = "Fallido";
            Response = false;
            Errors = null;

        }
        public ResponseLogin SetResponseLogin(int _id, bool _response, string _message, string _errors = null, string _nombres = null, string _userId = null)
        {
            Id = _id;
            UserId = _userId;
            Message = _message;
            Response = _response;
            Errors = _errors;
            Nombres = _nombres;
            return this;
        }
    }
}