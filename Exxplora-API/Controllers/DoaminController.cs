using Exxplora_API.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Exxplora_API.Models;
using Exxplora_API.Result;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/domain")]
    public class DoaminController : ApiController
    {
        [HttpGet]
        [Route("projects/{id:int}")]
        public Result<dynamic> GetProjectsByDomain(int id)
        {
            try
            {
                return new Result<dynamic>
                {
                    IsError = false,
                    Messages = new List<string> { "All projects" },
                    Data = DataAccess.DB.Domains
                              .Where(d => d.Id == id)
                              .Select(d => new
                              {
                                  d.Id,
                                  d.Name,
                                  Projects = d.Projects.Select(p => new
                                  {
                                      p.Id,
                                      p.Title,
                                      p.Description,
                                      p.CreatedDate,
                                      p.StartDate,
                                      p.EndDate,
                                      p.Budget,
                                      p.IsArchived,
                                      Domains = p.Domains.Select(pd => new
                                      {
                                          pd.Id,
                                          pd.Name

                                      }).ToList()
                                  }).ToList()
                              }).ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<dynamic> { IsError = true, Messages = new List<String> { "Something went wrong when try to connect with database", ex.Message }, Data = null };
            }
        }


    }
}
