using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Exxplora_API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Exxplora_API.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("api/authenticate")]
        public IHttpActionResult Authenticate([FromBody] AuthModel model)
        {

            var role = "User";
            if (model.Username == "admin")
            {
                role = "Admin";
            } //test

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(ConfigurationManager.AppSettings["jwtSecret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = ConfigurationManager.AppSettings["jwtIssuer"],
                Audience = ConfigurationManager.AppSettings["jwtAudience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

    }

}

