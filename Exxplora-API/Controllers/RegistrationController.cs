﻿using Exxplora_API.Models;
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
    public class RegistrationController : ApiController
    {
        [HttpPost]
        [Route("api/registration/user")]
        public Result<dynamic> RegisterUser([FromBody] User model)
        {
            if (model == null)
            {
                return new Result<dynamic> { IsError = true, Messages = new List<String> { "You must provide data" }, Data = null };

            }

            if (!ModelState.IsValid)
            {
                return new Result<dynamic>
                {
                    IsError = true,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList(),
                    Data = null
                };
            }

            var user = DataAccess.DB.Users.FirstOrDefault(u => u.Email.Equals(model.Email));
            if (user != null)
            {
                return new Result<dynamic> { IsError = true, Messages = new List<String> { "Email Already in Use. Please try with different email" }, Data = null };
            }

            
            
            try
            {
                model.RoleId = (int)Roles.USER;
                DataAccess.DB.Users.Add(model);
                DataAccess.DB.SaveChanges();
                return new Result<dynamic>
                {
                    IsError = false,
                    Messages = new List<String> { "User Registred" },
                    Data = new
                    {
                        model.FirstName,
                        model.LastName,
                        model.Email,
                        model.Phone,
                        model.Institute
                    }
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Result<dynamic> { IsError = true, Messages = new List<String> { "Something went wrong when try to connect with database", ex.Message }, Data = null };
            }
        }
    }
}
