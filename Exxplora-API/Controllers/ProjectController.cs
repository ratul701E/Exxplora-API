using Exxplora_API.Models;
using Exxplora_API.Models.DTO;
using Exxplora_API.Result;
using Exxplora_API.Static;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Exxplora_API.Utilities;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/project")]
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
                    AuthorId = int.Parse(ClaimsHelper.GetUserId(User)),
                    ProjectStatusId = 1,
                    CreatedDate = DateTime.Now,
                    StartDate = null,
                    EndDate = model.EndDate,
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
                return ResultHelper.ErrorResponse<dynamic>(new List<String> { "Something went wrong when try to connect with database", ex.Message }, ex);
            }
        }


        [HttpGet]
        [Authorize]
        public Result<List<Project>> GetAllProjects()
        {

            return ResultHelper.SuccessResponse("All Project Returned", DataAccess.DB.Projects.Include(p => p.Author).Include(p => p.Domains).Include(p=>p.ProjectStatus).ToList());
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public Result<Project> DeleteProject(int id)
        {
            try
            {
                var project = DataAccess.DB.Projects.FirstOrDefault(p => p.Id == id);
                if (project == null)
                {
                    return ResultHelper.ErrorResponse<Project>("Project not found");
                }

                DataAccess.DB.Projects.Remove(project);
                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Project deleted successfully", project);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Project>(new List<string> { "Failed to delete project", ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("{id:int}")]
        public Result<Project> UpdateProject(int id, [FromBody] ProjectDTO model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<Project>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<Project>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            try
            {
                var existingProject = DataAccess.DB.Projects.Include(p => p.Domains)
                                                            .FirstOrDefault(p => p.Id == id);
                if (existingProject == null)
                {
                    return ResultHelper.ErrorResponse<Project>("Project not found");
                }

                existingProject.Title = model.Title;
                existingProject.Description = model.Description;
                existingProject.EndDate = model.EndDate;
                existingProject.IsArchived = model.IsArchived;

                var updatedDomains = DataAccess.DB.Domains
                                     .Where(d => model.Domains.Contains(d.Id))
                                     .ToList();

                existingProject.Domains.Clear();
                foreach (var domain in updatedDomains)
                {
                    existingProject.Domains.Add(domain);
                }

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Project updated successfully", existingProject);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Project>(new List<string> { "Failed to update project", ex.Message });
            }
        }


    }
}
