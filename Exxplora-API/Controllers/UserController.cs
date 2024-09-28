using Exxplora_API.Models;
using Exxplora_API.Models.DTO;
using Exxplora_API.Result;
using Exxplora_API.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("update/{id:int}")]
        public Result<dynamic> UpdateUserInformation(int id, [FromBody] UpdateProfileDTO model)
        {

            if (model == null)
            {
                return new Result<dynamic> { IsError = true, Messages = new List<string> { "You must provide data" }, Data = null };

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
                var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.IsStudent = model.IsStudent;
                user.StartYear = model.StartYear;
                user.Institute = model.Institute;

                DataAccess.DB.SaveChanges();
                return new Result<dynamic> { IsError = false, Messages = new List<string> { "user updated" }, 
                    Data = new { 
                        model.FirstName,
                        model.LastName,
                        model.Email,
                        model.IsStudent,
                        model.StartYear,
                        model.Institute,
                    } 
                };
            }
            catch (Exception ex)
            {
                return new Result<dynamic> { IsError = true, Messages = new List<string> { "Something went wrong when try to connect with database", ex.Message }, Data = null };
            }
        }

        //[HttpPost]
        //[Route("/update-pp/{id:int}")]
        //public Result<bool?> UpdateProfilePicuture(int id)
        //{

        //}

        [HttpPost]
        [Route("upload-profile-picture/{id:int}")]
        public async Task<Result<bool?>> UploadProfilePicture(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new Result<bool?> { IsError = true, Data = null, Messages = new List<string> { "Unsupported media type" } };
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);

                if (user == null)
                {
                    return new Result<bool?> { IsError = true, Data = null, Messages = new List<string> { "User with id " + id.ToString() + " not found" } };
                }

                foreach (var file in provider.Contents)
                {
                    string fileExtension = Path.GetExtension(file.Headers.ContentDisposition.FileName.Trim('\"'));
                    string customFileName = $"{id}_profile_picture{fileExtension}";

                    var buffer = await file.ReadAsByteArrayAsync();

                    var uploadFolderPath = HttpContext.Current.Server.MapPath("~/UploadedFiles/profiles");

                    if (!Directory.Exists(uploadFolderPath))
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    var filePath = Path.Combine(uploadFolderPath, customFileName);

                    File.WriteAllBytes(filePath, buffer);

                    user.ProfilePicturePath = customFileName;
                    DataAccess.DB.SaveChanges();

                    return new Result<bool?> { IsError = false, Messages = new List<string> { "Profile uploaded successfully" }, Data = true };
                }

                return new Result<bool?> { IsError = true, Data = null, Messages = new List<string> { "File not received" } };
            }
            catch (Exception ex)
            {
                return new Result<bool?> { IsError = true, Data = null, Messages = new List<string> { ex.Message } };
            }
        }

        //[HttpGet]
        //[Route("get-profile-picture/{id:int}")]
        //public async Task<IHttpActionResult> GetProfilePicture(int id)
        //{

        //}
    }
}
