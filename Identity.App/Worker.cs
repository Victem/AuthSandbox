using Identity.App.Data;

using OpenIddict.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.App
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("gateway") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "gateway",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    //DisplayName = "Api gateway",
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.Password,

                    //    Permissions.GrantTypes.AuthorizationCode,
                    //    Permissions.ResponseTypes.Code,
                    //    Permissions.Scopes.Email,
                    //    Permissions.Scopes.Profile,
                    //    Permissions.Scopes.Roles
                    //},
                    //Requirements =
                    //{
                    //    Requirements.Features.ProofKeyForCodeExchange
                    //}
                    }
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
