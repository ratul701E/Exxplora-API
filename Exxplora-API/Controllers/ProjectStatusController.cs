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
    [RoutePrefix("api/project-status")]
    public class ProjectStatusController : ApiController
    {
        [HttpGet]
        [Route("")]
        public Result<List<ProjectStatus>> GetAllProjectStatus()
        {
            try
            {
                return ResultHelper.SuccessResponse("All Project Status with name and id", DataAccess.DB.ProjectStatuses.ToList());
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse< List<ProjectStatus>>(new List<string> { "Something went wrong when try to connect with database", ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("")]
        public Result<ProjectStatus> AddProjectStatus([FromBody] ProjectStatus model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<ProjectStatus>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<ProjectStatus>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            try
            {
                DataAccess.DB.ProjectStatuses.Add(model);
                DataAccess.DB.SaveChanges();
                return ResultHelper.SuccessResponse("Project Status added successfully", model);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<ProjectStatus>(new List<string> { "Failed to add project status", ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public Result<string> RemoveProjectStatus(int id)
        {
            try
            {
                var projectStatus = DataAccess.DB.ProjectStatuses.FirstOrDefault(p => p.Id == id);
                if (projectStatus == null)
                {
                    return ResultHelper.ErrorResponse<string>("Project Status not found");
                }

                DataAccess.DB.ProjectStatuses.Remove(projectStatus);
                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Project Status deleted successfully", "Success");
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<string>(new List<string> { "Failed to delete project status", ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public Result<ProjectStatus> UpdateProjectStatus(int id, [FromBody] ProjectStatus model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<ProjectStatus>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<ProjectStatus>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            try
            {
                var projectStatus = DataAccess.DB.ProjectStatuses.FirstOrDefault(p => p.Id == id);
                if (projectStatus == null)
                {
                    return ResultHelper.ErrorResponse<ProjectStatus>("Project Status not found");
                }

                projectStatus.Status = model.Status;

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Project Status updated successfully", projectStatus);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<ProjectStatus>(new List<string> { "Failed to update project status", ex.Message });
            }
        }

    }
}
