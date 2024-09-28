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
    [RoutePrefix("api/projectstatus")]
    public class ProjectStatusController : ApiController
    {
        [HttpGet]
        [Route("")]
        public Result<List<ProjectStatus>> GetAllProjectStatus()
        {
            try
            {
                return new Result<List<ProjectStatus>>
                {
                    IsError = false,
                    Messages = new List<string> { "All Project Status with name and id" },
                    Data = DataAccess.DB.ProjectStatuses.ToList()
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ProjectStatus>> { IsError = true, Messages = new List<string> { "Something went wrong when try to connect with database", ex.Message }, Data = null };
            }
        }
    }
}
