using Gateway.Api;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");
// Add services to the container.

builder.Services.AddOpenIddict()
    .AddClient(options => 
    {
        options.AllowClientCredentialsFlow()
            .AllowPasswordFlow();
        
        options.DisableTokenStorage();
        
        options.UseSystemNetHttp()
            .SetProductInformation(typeof(Program).Assembly);

        options.UseAspNetCore(options =>
        {
            
        });

        options.AddRegistration(new OpenIddict.Client.OpenIddictClientRegistration
        {
            Issuer = new Uri("https://localhost:7299/", UriKind.Absolute),
            ClientId = "gateway",
            ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
            GrantTypes = 
            { 
                Permissions.GrantTypes.ClientCredentials,
                Permissions.GrantTypes.Password
            }
        });
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot()
    .AddDelegatingHandler<AuthenticationHandler>()
    .AddDelegatingHandler<AuthorizationHandler>();


builder.Services.AddSingleton<SessionStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseOcelot();
app.Run();
