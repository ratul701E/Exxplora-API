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
        private User GetUserById(int id)
        {
            return DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);
        }

        private string GetMimeType(string fileExtension)
        {
            if (fileExtension == "png")
            {
                return "image/png";
            }
            else if (fileExtension == "jpg" || fileExtension == "jpeg")
            {
                return "image/jpeg";            }
            else if (fileExtension == "gif")
            {
                return "image/gif";
            }
            else
            {
                return "application/octet-stream";
            }
        }

        private IHttpActionResult ReturnFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return NotFound();
            }

            var imageData = File.ReadAllBytes(filePath);
            var fileExtension = Path.GetExtension(filePath).TrimStart('.');
            var mimeType = GetMimeType(fileExtension);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imageData)
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

            return ResponseMessage(response);
        }

        private async Task<Result<bool?>> HandleFileUpload(int id, string folderPath, string fileType)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return ResultHelper.ErrorResponse<bool?>("Unsupported media type");
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var user = GetUserById(id);

            if (user == null)
            {
                return ResultHelper.ErrorResponse<bool?>($"User with id {id} not found");
            }

            foreach (var file in provider.Contents)
            {
                string fileExtension = Path.GetExtension(file.Headers.ContentDisposition.FileName.Trim('\"'));
                string customFileName = $"{id}_{fileType}{fileExtension}";
                var buffer = await file.ReadAsByteArrayAsync();

                var uploadFolderPath = HttpContext.Current.Server.MapPath(folderPath);
                Directory.CreateDirectory(uploadFolderPath);

                var filePath = Path.Combine(uploadFolderPath, customFileName);
                File.WriteAllBytes(filePath, buffer);

                if (fileType == "profile_picture")
                {
                    user.ProfilePicturePath = customFileName;
                }
                else if (fileType == "cover_photo")
                {
                    user.CoverPhotoPath = customFileName;
                }

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse<bool?>("File uploaded successfully", true);
            }

            return ResultHelper.ErrorResponse<bool?>("File not received");
        }

        [HttpPost]
        [Route("update/{id:int}")]
        public Result<dynamic> UpdateUserInformation(int id, [FromBody] UpdateProfileDTO model)
        {
            if (model == null)
            {
                return ResultHelper.ErrorResponse<dynamic>("You must provide data");
            }

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return ResultHelper.ErrorResponse<dynamic>(errorMessages);
            }

            try
            {
                var user = GetUserById(id);
                if (user == null)
                {
                    return ResultHelper.ErrorResponse<dynamic>("User not found");
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.IsStudent = model.IsStudent;
                user.StartYear = model.StartYear;
                user.Institute = model.Institute;

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse<dynamic>("User updated", new
                {
                    model.FirstName,
                    model.LastName,
                    model.Email,
                    model.IsStudent,
                    model.StartYear,
                    model.Institute,
                });
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<dynamic>($"Something went wrong when trying to connect to the database: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("upload-profile-picture/{id:int}")]
        public Task<Result<bool?>> UploadProfilePicture(int id)
        {
            return HandleFileUpload(id, "~/UploadedFiles/profiles", "profile_picture");
        }

        [HttpPost]
        [Route("upload-cover-photo/{id:int}")]
        public Task<Result<bool?>> UploadCoverPhoto(int id)
        {
            return HandleFileUpload(id, "~/UploadedFiles/covers", "cover_photo");
        }

        [HttpGet]
        [Route("get-profile-picture/{id:int}")]
        public IHttpActionResult GetProfilePicture(int id)
        {
            var user = GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var path = HttpContext.Current.Server.MapPath("~/UploadedFiles/profiles/" + user.ProfilePicturePath);
            return ReturnFile(path);
        }

        [HttpGet]
        [Route("get-cover-photo/{id:int}")]
        public IHttpActionResult GetCoverPhoto(int id)
        {
            var user = GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var path = HttpContext.Current.Server.MapPath("~/UploadedFiles/covers/" + user.CoverPhotoPath);
            return ReturnFile(path);
        }
    }
}
