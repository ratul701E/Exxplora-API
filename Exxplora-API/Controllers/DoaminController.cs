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
        [Route("")]
        public Result<List<Domain>> GetAllDomains()
        {
            try
            {
                return ResultHelper.SuccessResponse("All domain with name and id", DataAccess.DB.Domains.ToList());
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse< List<Domain>>(new List<string> { "Something went wrong when try to connect with database", ex.Message });
            }
        }

        [HttpGet]
        [Route("projects/{id:int}")]
        public Result<dynamic> GetProjectsByDomain(int id)
        {
            try
            {
                return ResultHelper.SuccessResponse<dynamic>("All projects", DataAccess.DB.Domains
                              .Where(d => d.Id == id)
                              .Select(d => new
                              {
                                  d.Id,
                                  d.Name,
                                  d.Description,
                                  Projects = d.Projects.Select(p => new
                                  {
                                      p.Id,
                                      p.Title,
                                      p.Description,
                                      p.CreatedDate,
                                      p.StartDate,
                                      p.EndDate,
                                      p.IsArchived,
                                      Domains = p.Domains.Select(pd => new
                                      {
                                          pd.Id,
                                          pd.Name

                                      }).ToList()
                                  }).ToList()
                              }).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ResultHelper.ErrorResponse<dynamic>(new List<String> { "Something went wrong when try to connect with database", ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("")]
        public Result<Domain> CreateDomain(Domain model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<Domain>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<Domain>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            try
            {
                DataAccess.DB.Domains.Add(model);
                DataAccess.DB.SaveChanges();
                return ResultHelper.SuccessResponse("Domain added", model);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Domain>(new List<string> { "Failed to create section", ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public Result<Domain> DeleteDomain(int id)
        {
            try
            {
                var domain = DataAccess.DB.Domains.FirstOrDefault(d => d.Id == id);
                if (domain == null)
                {
                    return ResultHelper.ErrorResponse<Domain>("Domain not found");
                }

                DataAccess.DB.Domains.Remove(domain);
                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Domain deleted successfully", domain);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Domain>(new List<string> { "Failed to delete domain", ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public Result<Domain> UpdateDomain(int id, [FromBody] Domain model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<Domain>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<Domain>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            try
            {
                var existingDomain = DataAccess.DB.Domains.FirstOrDefault(d => d.Id == id);
                if (existingDomain == null)
                {
                    return ResultHelper.ErrorResponse<Domain>("Domain not found");
                }

                existingDomain.Name = model.Name;
                existingDomain.Description = model.Description;

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Domain updated successfully", existingDomain);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Domain>(new List<string> { "Failed to update domain", ex.Message });
            }
        }


    }
}
