using Gateway.Api.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gateway.Api
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly SessionStore _sessionStore;

        public AuthorizationHandler(SessionStore sessionStore)
        {
            _sessionStore = sessionStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
        {
            if (request.Method == HttpMethod.Post)
            {
                var contentString = await request.Content.ReadAsStringAsync(cancellationToken);
                var session = JsonSerializer.Deserialize<PickpointSession>(contentString);
                
                if (session is not null && !string.IsNullOrEmpty(session.SessionId))
                {
                    var authClaims = _sessionStore.GetSesssion(session.SessionId);

                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized };
                }
            }

            var result = await base.SendAsync(request, cancellationToken);
            return result;
        }
    }
}
