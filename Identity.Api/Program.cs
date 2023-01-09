using Identity.Api;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using OpenIddict;
using OpenIddict.Core;
using OpenIddict.Server.AspNetCore;
using OpenIddict.EntityFrameworkCore;
using System.Configuration;
using System.Security.Claims;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<IdentityDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDb"));
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict()
    .AddCore(options => 
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<IdentityDbContext>();
        
        
    })
    .AddServer(options =>
    {
        options.UseAspNetCore();
        options.SetAuthorizationEndpointUris("/login");
        
        options.AllowAuthorizationCodeFlow();
        
    })
    
    .AddValidation(options => 
    { 
        options.UseLocalServer();
        options.UseAspNetCore();
    });

var app = builder.Build();

app.MapGet("/", async (context) => 
{
    
    await Task.FromResult( Results.Ok(context));
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
