using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.ResponseImageWS
{

    public class ResponseWSimage
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        [JsonProperty("guideInfo")]
        public GuideInfo GuideInfo { get; set; }
        public bool Success { get; set; }
    }

    public class ResponseWSGetImage
    {
        public string ImageBase64 { get; set; }

        public bool Success { get; set; }
    }

    public class GuideInfo
    {
        [JsonProperty("sender")]
        public Sender Sender { get; set; }
        [JsonProperty("receiver")]
        public Receiver Receiver { get; set; }
        [JsonProperty("complete")]
        public bool Complete { get; set; }
    }
    public class Sender
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phoneNumber")]
        public string Cell { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }

    }
    public class Receiver
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phoneNumber")]
        public string Cell { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
