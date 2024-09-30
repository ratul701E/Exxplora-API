using Exxplora_API.Models;
using Exxplora_API.Result;
using Exxplora_API.Static;
using Exxplora_API.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/version")]
    public class VersionController : ApiController
    {
        [HttpPost]
        [Authorize]
        public Result<Version> CreateVersion([FromBody] Version model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<Version>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                return ResultHelper.ErrorResponse<Version>(
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                    );
            }

            

            try
            {
                var section = DataAccess.DB.Sections.FirstOrDefault(s => s.Id == model.SectionId);
                if (section == null)
                {
                    return ResultHelper.ErrorResponse<Version>("Section is not exist");
                }

                model.CreatedDate = System.DateTime.Now;
                model.CreatedBy = int.Parse(ClaimsHelper.GetUserId(User));


                DataAccess.DB.Versions.Add(model);
                DataAccess.DB.SaveChanges();
                return ResultHelper.SuccessResponse("Version Created", model);

            }
            catch (System.Exception ex)
            {
                return ResultHelper.ErrorResponse<Version>(new List<string> { "Failed to create version", ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public Result<List<Version>> GetVersions(int id)
        {
            try
            {
                var section = DataAccess.DB.Sections.FirstOrDefault(s => s.Id == id);
                if (section == null)
                {
                    return ResultHelper.ErrorResponse<List<Version>>("No Section found");
                }

                var versions = DataAccess.DB.Versions.Where(v=> v.SectionId == id).ToList();
                return ResultHelper.SuccessResponse("All Version of section id " + id, versions);
            }
            catch (System.Exception ex)
            {
                return ResultHelper.ErrorResponse<List<Version>>(new List<string> { "Failed to get versions", ex.Message });
            }
        }
    }
}
