using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Services
{
    public class JwtSettings
    {
        public SigningCredentials SigningCredentials { get; }

        public JwtSettings(IConfiguration configuration)
        {
            var signingKey = configuration.GetSection("JwtSettings:SigningKey").Value;
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        }


    }
}
