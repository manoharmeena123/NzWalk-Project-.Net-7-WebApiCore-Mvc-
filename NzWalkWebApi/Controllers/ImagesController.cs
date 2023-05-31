using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NzWalkWebApi.Models.Domain;
using NzWalkWebApi.Models.DTO;
using NzWalkWebApi.Repositories;

namespace NzWalkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST:/api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUplad(request);
            if (ModelState.IsValid)
            {

                // Convert DTO to Domain Model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };


                //User repositry to upload image
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);

        }


        private void ValidateFileUplad (ImageUploadRequestDto request)
        {
            var allowExtension = new string[] { ".jpg", ".jpeg", ".png", ".jfif" };
            if(!allowExtension.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unaupported file extension");
            }
            if(request.File.Length > 10485760) {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file. ");
            }
        }

    }
}
