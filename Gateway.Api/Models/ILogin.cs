using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Models
{
    public interface ILogin
    {
        string Name { get; set; }
        string Password { get; set; }
    }

    public class LoginDto : ILogin
    {
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
