using CapturaCognitiva.App_Tools;

namespace CapturaCognitiva.Models.Response
{
    public class ResponseRecoveryPassword : ResponseHelper
    {
        public ResponseRecoveryPassword()
        {
            Message = "Fallido";
            Response = false;
            Errors = null;

        }
        public ResponseRecoveryPassword SetResponseRecoveryPassword(int _id, bool _response, string _message, string _errors = null)
        {
            Id = _id;           
            Message = _message;
            Response = _response;
            Errors = _errors;           
            return this;
        }
    }
}