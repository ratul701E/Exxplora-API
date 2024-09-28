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
                return ResultHelper.ErrorResponse<dynamic>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<dynamic>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
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
                    Domains = DataAccess.DB.Domains
                         .Where(d => model.Domains.Contains(d.Id))
                         .ToList()
                };

                DataAccess.DB.Projects.Add(project);
                DataAccess.DB.SaveChanges();
                return ResultHelper.SuccessResponse<dynamic>("Project Added", project);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ResultHelper.ErrorResponse<dynamic>(new List<String> { "Something went wrong when try to connect with database", ex.Message });
            }
        }


        [HttpGet]
        [Authorize]
        public Result<List<Project>> GetAllProjects()
        {

            return ResultHelper.SuccessResponse("All Project Returned", DataAccess.DB.Projects.Include(p => p.Author).ToList());
        }

    }
}
