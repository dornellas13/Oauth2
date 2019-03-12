using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OAuth.Services;

namespace OAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuaration;
        private readonly JwtSettings _jwtSettings;
        public TokenController(IConfiguration configuration, JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _configuaration = configuration;
        }

        // GET api/authorize
        [HttpGet(), AllowAnonymous]
        public ActionResult<ActionResult<object>> Get()
        {

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                // Claims
                Subject = GetClaimsIdentity(),
                // Issuer (iss): quem emite o token JWT;
                Issuer = _configuaration.GetSection("JwtSettings:Issuer").Value,
                /// Audience (aud): aplicações que podem usar o token JWT. 
                Audience = _configuaration.GetSection("JwtSettings:Audience").Value,
                // IssuedAt (iat): data e hora em que o token foi emitido;
                IssuedAt = DateTime.Now,
                // Expires (exp): data e hora em que o token irá expirar;
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = _jwtSettings.SigningCredentials
            });

            return Ok(new
            {
                access_token = handler.WriteToken(jwtToken),
                token_type = "bearer",
            });
        }



        private ClaimsIdentity GetClaimsIdentity()
        {
            return new ClaimsIdentity
            (
                new GenericIdentity("dornellas13@gmail.com"),
                new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, "Marcos Dornellas")
                }
            );
        }

    }
}