using Exxplora_API.Models;
using Exxplora_API.Models.DTO;
using Exxplora_API.Result;
using Exxplora_API.Static;
using Exxplora_API.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Exxplora_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {

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

        private IHttpActionResult ReturnFile(string filePath, bool isProfile = true)
        {
            if (!File.Exists(filePath))
            {
                if(isProfile) filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/profiles/default.png");
                else filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/covers/default.png");
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
            var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);

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
        [Authorize]
        [Route("setup")]
        public Result<dynamic> SetupUserInformation([FromBody] ProfileSetupDTO model)
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
                var id = int.Parse(ClaimsHelper.GetUserId(User));
                var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);
                if (user == null)
                {
                    return ResultHelper.ErrorResponse<dynamic>("User not found");
                }

                user.Location = model.Location;
                user.EndYear = model.EndYear;
                user.IsStudent = model.IsStudent;
                user.StartYear = model.StartYear;
                user.Institute = model.Institution;
                user.Domains = DataAccess.DB.Domains
                         .Where(d => model.Domains.Contains(d.Id))
                         .ToList();

                DataAccess.DB.SaveChanges();

                return ResultHelper.SuccessResponse<dynamic>("User updated", new
                {
                    model.Location,
                    model.IsStudent,
                    model.StartYear,
                    model.EndYear,
                    model.Institution,
                    model.Domains
                });
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<dynamic>($"Something went wrong when trying to connect to the database: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("upload-profile-picture")]
        public Task<Result<bool?>> UploadProfilePicture()
        {
            try
            {
                var id = int.Parse(ClaimsHelper.GetUserId(User));
                return HandleFileUpload(id, "~/UploadedFiles/profiles", "profile_picture");
            }
            catch (Exception ex)
            {
                return Task.FromResult(ResultHelper.ErrorResponse<bool?>("Failed to patch id"));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("upload-cover-photo")]
        public Task<Result<bool?>> UploadCoverPhoto()
        {
            try
            {
                var id = int.Parse(ClaimsHelper.GetUserId(User));
                return HandleFileUpload(id, "~/UploadedFiles/covers", "cover_photo");
            }
            catch (Exception ex)
            {
                return Task.FromResult(ResultHelper.ErrorResponse<bool?>("Failed to patch id"));
            }
        }

        [HttpGet]
        [Route("get-profile-picture/{id:int}")]
        public IHttpActionResult GetProfilePicture(int id)
        {
            var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);
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
            var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            var path = HttpContext.Current.Server.MapPath("~/UploadedFiles/covers/" + user.CoverPhotoPath);
            return ReturnFile(path, false);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public Result<GetInfoDTO> GetInfo()
        {
            try
            {
                var id = int.Parse(ClaimsHelper.GetUserId(User));
                var user = DataAccess.DB.Users.FirstOrDefault(u => u.ID == id);
                GetInfoDTO getInfoDTO = new GetInfoDTO
                {
                    Id = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    OrganizationName = user.Institute,
                    Location = user.Location,
                    ProfilePicUrl = "http://localhost:65015/api/user/get-profile-picture/" + user.ID,
                    CoverPicUrl = "http://localhost:65015/api/user/get-cover-photo/" + user.ID,
                };

                return ResultHelper.SuccessResponse("Data retrive successfull", getInfoDTO);
            }
            catch (Exception ex)
            {
                return ResultHelper.ErrorResponse<GetInfoDTO>("Something is wrong");
            }
        }

        //[HttpGet]
        //[Authorize]
        //[Route("getmyphoto")]
        //public IHttpActionResult GeTMyPhoto()
        //{
        //    var identity = User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        // Extract the email claim
        //        var emailClaim = identity.FindFirst(ClaimTypes.Email) ??
        //                         identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        //        var email = emailClaim?.Value;
        //        var path = DataAccess.DB.Users.FirstOrDefault(x => x.Email == email).ProfilePicturePath;
        //        path = HttpContext.Current.Server.MapPath("~/UploadedFiles/profiles/" + path);
        //        return ReturnFile(path);
        //    }
        //    return null;
        //}

        //[HttpGet]
        //[Authorize]
        //[Route("getmycover")]
        //public IHttpActionResult GeTMyCover()
        //{
        //    var identity = User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        // Extract the email claim
        //        var emailClaim = identity.FindFirst(ClaimTypes.Email) ??
        //                         identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        //        var email = emailClaim?.Value;
        //        var path = DataAccess.DB.Users.FirstOrDefault(x => x.Email == email).CoverPhotoPath;
        //        path = HttpContext.Current.Server.MapPath("~/UploadedFiles/covers/" + path);
        //        return ReturnFile(path);
        //    }
        //    return null;
        //}
    }
}
