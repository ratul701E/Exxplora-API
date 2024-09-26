using Exxplora_API.Models;
using Exxplora_API.Models.DTO;
using Exxplora_API.Result;
using Exxplora_API.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace Exxplora_API.Controllers
{
    [Route("api/project")]
    public class ProjectController : ApiController
    {
        [HttpPost]
        [Authorize]
        public Result<dynamic> CreateProject([FromBody] ProjectDTO model)
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

            try
            {
                var project = new Project
                {
                    Title = model.Title,
                    Description = model.Description,
                    AuthorId = model.AuthorId,
                    ProjectStatusId = 1,
                    CreatedDate = DateTime.Now,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Budget = model.Budget,
                    IsArchived = false,
                    Contributors = new List<User>(),
                    Domains = DataConnection.DB.Domains
                         .Where(d => model.Domains.Contains(d.Id))
                         .ToList()
                };

                DataConnection.DB.Projects.Add(project);
                DataConnection.DB.SaveChanges();
                return new Result<dynamic> { IsError = false, Messages = new List<String> { "Project Added" }, Data = project };

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<dynamic> { IsError = true, Messages = new List<String> { "Something went wrong when try to connect with database", ex.Message }, Data = ex };
            }
        }


        [HttpGet]
        public Result<List<Project>> GetAllProjects()
        {
            return new Result<List<Project>> { 
                IsError = false,
                Messages = new List<String> { "All Project Returned" },
                Data = DataConnection.DB.Projects.Include(p => p.Author).ToList()

            };
        }

    }
}
