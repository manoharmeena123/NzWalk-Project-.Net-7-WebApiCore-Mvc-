using NzWalkWebApi.Models.Domain;

namespace NzWalkWebApi.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
