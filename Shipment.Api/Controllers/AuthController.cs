using Microsoft.AspNetCore.Mvc;

using Shipment.Api.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("~/login")]
        public IActionResult Login([FromBody] LoginDto loginData)
        {
            return Ok(new { SessionId = Guid.NewGuid().ToString().ToUpperInvariant() });
        }
    }
}
