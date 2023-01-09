using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gateway.Api.Models
{
    public interface IPickpointSession
    {
        string SessionId { get; set; }
    }

    public class PickpointSession : IPickpointSession
    {
        [JsonPropertyName("sessionId")]
        public string? SessionId { get; set; }
    }
}
