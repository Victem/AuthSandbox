using Identity.App.Areas.Identity.Models;
using Identity.App.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.App.Areas.Identity.Controllers
{
    [ApiController]
    [Route("[area]/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("/verify-user")]
        //[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyObsoleteUser([FromBody] LoginDto loginData)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(loginData.Name);
            if (user == null) 
            {
                user = new ApplicationUser { UserName = loginData.Name };
                var result = await _userManager.CreateAsync(user, loginData.Password);
                await _userManager.AddClaimAsync(user, new Claim(Permissions.Prefixes.Scope + "sheepla", Permissions.Prefixes.Endpoint + "create"));
            }
            return Ok();
        }
    }
}
