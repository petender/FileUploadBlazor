using FileUploadBlazor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace FileUploadBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
        {
            List<UploadResult> uploadResults = new List<UploadResult>();
            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameforFileStorage;
                var untrustedfileName = file.FileName;
                uploadResult.FileName = untrustedfileName;
                var trustedfileNameforDisplay = WebUtility.HtmlEncode(untrustedfileName);

                trustedFileNameforFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "Uploads", trustedFileNameforFileStorage);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

                uploadResult.StoredFileName = trustedFileNameforFileStorage;
                uploadResults.Add(uploadResult);

            }
            return Ok(uploadResults);
        }
    }   
}
