using CapturaCognitiva.App_Tools;
using CapturaCognitiva.Models.ResponseImageWS;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.WebServices
{
    public class WSImage
    {
        private string EndPoint { get; set; }

        public WSImage()
        {
            EndPoint = ConfigurationManager.AppSetting["EndPointWS:ImageAWS"];
        }

        public ResponseWSimage AnalyserImage(string imageBase64)
        {
            try
            {
                var client = new RestClient(string.Concat(EndPoint, "analyzer"))
                {
                    Timeout = -1
                };
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-File-Extension", "jpg");
                request.AddHeader("Content-Type", "text/plain");
                request.AddParameter("text/plain", imageBase64, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var objResponse = JsonConvert.DeserializeObject<ResponseWSimage>(response.Content);
                    objResponse.Success = true;
                    return objResponse;
                }
                else
                {
                    return new ResponseWSimage { GuideInfo = null, Uuid = null, Success = false };
                }
            }
            catch
            {
                return new ResponseWSimage { GuideInfo = null, Uuid = null, Success = false };
                throw;
            }

        }

        public ResponseWSGetImage GetImage(string Uuid)
        {
            try
            {
                var client = new RestClient(string.Concat(EndPoint, Uuid))
                {
                    Timeout = -1
                };
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return new ResponseWSGetImage { ImageBase64 = response.Content, Success = true };
                }
                else
                {
                    return new ResponseWSGetImage { ImageBase64 = null, Success = false };
                }
            }
            catch
            {
                return new ResponseWSGetImage { ImageBase64 = null, Success = false };
                throw;
            }

        }
    }
}
