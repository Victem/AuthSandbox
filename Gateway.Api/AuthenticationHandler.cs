using Gateway.Api.Models;

using IdentityServer4.Models;

using OpenIddict.Client;

using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Gateway.Api
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly OpenIddictClientService _openIdClient;
        private readonly HttpClient _httpClient;
        private readonly SessionStore _sessionStore;
        public AuthenticationHandler(OpenIddictClientService openIdClient, HttpClient httpClient, SessionStore sessionStore)
        {
            _openIdClient = openIdClient;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var isAuthorized = false;
            //if (request.Method == HttpMethod.Post) 
            //{ 
            //    var content = await request.Content.ReadAsStringAsync();
            //    var content2 = await request.Content.ReadFromJsonAsync<JsonDocument>();
            //    var rootElement = content2.RootElement.GetProperty("id").GetInt32();
            //    isAuthorized= rootElement == 100;
            //}
            //if (request.Method == HttpMethod.Get)
            //{
            //    isAuthorized = request.RequestUri.Segments[2] == "100";
            //}
            ////var id = content2[0];
            //if (!isAuthorized)
            //{
            //    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized };
            //}



            var loginBodyContent = await request.Content.ReadAsStringAsync(cancellationToken);
                      
            var loginBody = JsonSerializer.Deserialize<LoginDto>(loginBodyContent);
            var gatewayToken = await AuthorizeGateway();
            await VerifyUser(loginBody, gatewayToken);

            var result = await base.SendAsync(request, cancellationToken);
            var content = await result.Content.ReadAsStringAsync(cancellationToken);
            var session = JsonSerializer.Deserialize<PickpointSession>(content);
            //var session = await result.Content.ReadFromJsonAsync<PickpointSession>(options: null, cancellationToken);



            var userPermissions = await GetUserClaims(loginBody);
            _sessionStore.SaveSession(session.SessionId, userPermissions);
            return result;
        }


        private async Task<string> AuthorizeGateway()
        {
            var gatewayAuthResult = await _openIdClient.AuthenticateWithClientCredentialsAsync(new Uri("https://localhost:7299/", UriKind.Absolute));
            return gatewayAuthResult.Response.AccessToken;
        }

        private async Task VerifyUser(LoginDto loginData, string gatewayToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gatewayToken);
            var verifyUserResult = await _httpClient.PostAsJsonAsync("https://localhost:7299/verify-user", loginData);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private async Task<Dictionary<string, string>> GetUserClaims(LoginDto loginData)
        {
            var userTokenResult = await _openIdClient.AuthenticateWithPasswordAsync(
                new Uri("https://localhost:7299/", UriKind.Absolute),
                loginData.Name,
                loginData.Password
            );

            var userPermissions = userTokenResult.Principal.Claims
                .ToDictionary(key => key.Type, value => value.Value);
            return userPermissions;
        }
    }
}
