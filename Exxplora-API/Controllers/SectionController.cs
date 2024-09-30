using Exxplora_API.Models;
using Exxplora_API.Result;
using Exxplora_API.Static;
using Exxplora_API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/section")]
    public class SectionController : ApiController
    {
        [HttpPost]
        [Authorize]
        public Result<Section> CreateSection([FromBody] Section model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<Section>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<Section>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            try
            {
                var section = DataAccess.DB.Projects.FirstOrDefault(p => p.Id == model.ProjectId);
                if (section == null)
                {
                    return ResultHelper.ErrorResponse<Section>("Project does not exist");
                }

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = int.Parse(ClaimsHelper.GetUserId(User));

                DataAccess.DB.Sections.Add(model);
                DataAccess.DB.SaveChanges();
                return ResultHelper.SuccessResponse("Section Created", model);

            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Section>(new List<string> { "Failed to create section", ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public Result<List<Section>> GetSections(int id)
        {
            try
            {
                var project = DataAccess.DB.Projects.FirstOrDefault(s => s.Id == id);
                if (project == null)
                {
                    return ResultHelper.ErrorResponse<List<Section>>("No Project found");
                }
                var sections = DataAccess.DB.Sections.Where(u => u.ProjectId == id).ToList();
                return ResultHelper.SuccessResponse("All sections of project id " + id, sections);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<List<Section>>(new List<string> { "Failed to get sections", ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("{id:int}")]
        public Result<Section> UpdateSection(int id, [FromUri] string title)
        {
            if (title == null)
            {
                return ResultHelper.ErrorResponse<Section>("You must provide data");
            }

            try
            {
                var section = DataAccess.DB.Sections.FirstOrDefault(s => s.Id == id);
                if (section == null)
                {
                    return ResultHelper.ErrorResponse<Section>("Section not found");
                }

                section.Title = title;

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Section Updated Successfully", section);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<Section>(new List<string> { "Failed to update section", ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public Result<string> DeleteSection(int id)
        {
            try
            {
                var section = DataAccess.DB.Sections.FirstOrDefault(s => s.Id == id);
                if (section == null)
                {
                    return ResultHelper.ErrorResponse<string>("Section not found");
                }

                DataAccess.DB.Sections.Remove(section);
                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse("Section deleted successfully", "Success");
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<string>(new List<string> { "Failed to delete section", ex.Message });
            }
        }
    }
}
