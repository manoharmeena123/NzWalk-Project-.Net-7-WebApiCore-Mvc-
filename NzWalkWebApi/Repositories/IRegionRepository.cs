using NzWalkWebApi.Models.Domain;

namespace NzWalkWebApi.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
    }
}
