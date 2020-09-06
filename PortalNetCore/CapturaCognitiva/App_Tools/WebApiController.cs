using CapturaCognitiva.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;


namespace CapturaCognitiva.App_Tools
{

    public class WebApiController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool ValidarTokenWeb(string token)
        {
            try
            {
                string _Token = ConfigurationManager.AppSetting["CapturaCogninitvaKeys:App_key"];
                if (!string.IsNullOrEmpty(token))
                {
                    if (token == _Token)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetErroresModelo(ModelStateDictionary model)
        {
            string errores = "";
            var errors = model.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
            foreach (var item in errors)
            {
                errores += item;
            }
            return errores;
        }


    }
}
