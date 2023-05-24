using Microsoft.EntityFrameworkCore;
using NzWalkWebApi.Data;
using NzWalkWebApi.Models.Domain;

namespace NzWalkWebApi.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public async Task<List<Region>> GetAllAsync()
        {
           return await dbContext.Regions.ToListAsync();

        }

    }
}
