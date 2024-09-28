using Exxplora_API.Models;
using Exxplora_API.Result;
using Exxplora_API.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/registration")]
    public class RegistrationController : ApiController
    {
        [HttpPost]
        [Route("user")]
        public Result<dynamic> RegisterUser([FromBody] User model)
        {
            if (model == null)
            {
                ResultHelper.ErrorResponse<dynamic>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                ResultHelper.ErrorResponse<dynamic>(
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                );
            }

            var user = DataAccess.DB.Users.FirstOrDefault(u => u.Email.Equals(model.Email));
            if (user != null)
            {
                return ResultHelper.ErrorResponse<dynamic>("Email Already in Use. Please try with different email");
            }

            
            
            try
            {
                model.RoleId = (int)Roles.USER;
                DataAccess.DB.Users.Add(model);
                DataAccess.DB.SaveChanges();
                return ResultHelper.SuccessResponse<dynamic>("User Registred", 
                    new
                    {
                        model.FirstName,
                        model.LastName,
                        model.Email,
                    }
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultHelper.ErrorResponse<dynamic>(new List<string> { "Something went wrong when try to connect with database", ex.Message });
            }
        }

        [HttpGet]
        [Route("check-email-existance")]
        public Result<bool?> CheckEmailExist([FromUri] string email)
        {
            if (email == null)
            {
                return ResultHelper.ErrorResponse<bool?>("Invalid Request. Please Provide email");
            }


            if (this.CheckEmail(email))
            {
                return ResultHelper.SuccessResponse<bool?>("Email Alreay Used. Please try with different email", true);
            }
            return ResultHelper.SuccessResponse<bool?>("Email can be used.", false);

        }

        private bool CheckEmail(string email)
        {
            var user = DataAccess.DB.Users.FirstOrDefault(u => u.Email.Equals(email));
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
