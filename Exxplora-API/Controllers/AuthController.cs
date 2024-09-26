using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Exxplora_API.Models;
using Exxplora_API.Result;
using Exxplora_API.Static;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;


namespace Exxplora_API.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("api/authenticate")]
        public Result<string> Authenticate([FromBody] SigninDTO model)
        {

            
            
            if (model == null)
            {
                return new Result<string> { IsError = true, Messages = new List<String> { "You must provide data"}, Data = null };

            }

            if (!ModelState.IsValid)
            {
                return new Result<string> { 
                    IsError = true, 
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList(), 
                    Data = null 
                };
            }

            var user = DataConnection.DB.Users.Include(u => u.Role).FirstOrDefault(u => u.Email.Equals(model.Email) && u.Password.Equals(model.Password));

            if (user == null)
            {
                return new Result<string> { IsError = true, Messages = new List<String> { "Invalid Credentials" }, Data = null };
            }



            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(ConfigurationManager.AppSettings["jwtSecret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = ConfigurationManager.AppSettings["jwtIssuer"],
                Audience = ConfigurationManager.AppSettings["jwtAudience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new Result<string> { IsError = false, Messages = new List<String> { "Successfull" }, Data = tokenString};
        }

        [HttpGet]
        [Authorize]
        [Route("api/authenticate")]
        public List<User> TestAuth()
        {

            return DataConnection.DB.Users.Include(u => u.Role).ToList();
        }

    }

}

