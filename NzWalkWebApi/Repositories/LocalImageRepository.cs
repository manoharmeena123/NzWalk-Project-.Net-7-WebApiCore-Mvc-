using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NzWalkWebApi.Data;
using NzWalkWebApi.Models.Domain;

using Microsoft.AspNetCore.Hosting;

namespace NzWalkWebApi.Repositories
{
    public class LocalImageRepository : IImageRepository
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAcceser;
        private readonly NZWalksDbContext dbContext;
       public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAcceser, NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAcceser = httpContextAcceser;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            //Upload Image to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            /////https://loacalhost:1234/images/images.jpg

            var urlFilePath = $"{httpContextAcceser.HttpContext.Request.Scheme}://{httpContextAcceser.HttpContext.Request.Host}{httpContextAcceser.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            //Add Image To The IMAGE
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }

    }
}
