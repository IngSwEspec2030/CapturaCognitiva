using System;


namespace CapturaCognitiva.App_Tools
{
    public static class Encryptor
    {
        public static string GeneratePassword()
        {
            int longitud = 20;
            Guid miGuid = Guid.NewGuid();
            string password = Convert.ToBase64String(miGuid.ToByteArray());
            password = password.Replace("=", "").Replace("+", "");
            string passfinal = password.Substring(0, longitud);
            return passfinal;
        }
    }
}