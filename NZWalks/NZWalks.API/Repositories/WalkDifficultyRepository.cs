using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.walkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existimgWalkDifficulty = await nZWalksDbContext.walkDifficulty.FindAsync(id);
            if (existimgWalkDifficulty == null)
            {
                return null;
            };
            nZWalksDbContext.walkDifficulty.Remove(existimgWalkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return existimgWalkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
         return await   nZWalksDbContext.walkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetByIdAsync(Guid id)
        {
            return await nZWalksDbContext.walkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existimgWalkDifficulty = await nZWalksDbContext.walkDifficulty.FindAsync(id);
            if (existimgWalkDifficulty == null)
            {
                return null;
            }
            existimgWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existimgWalkDifficulty;  
        }
    }
}
