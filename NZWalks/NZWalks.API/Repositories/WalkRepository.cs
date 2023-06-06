using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign new Id
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
               //Find walk
            var existingWalk = await nZWalksDbContext.walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null)
            {
                return null;
            }

         nZWalksDbContext.walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;

        }

        public async Task<IEnumerable<Walk>>GetAllAsync()
        {
            return await nZWalksDbContext.walks
                .Include(x => x.Region).
                Include(x => x.walkDifficulty)
                .ToListAsync();

        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
         return await nZWalksDbContext.walks
                .Include(x => x.Region).
                Include(x => x.walkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //Find walk
            var existingWalk = await nZWalksDbContext.walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null)
            {
                return null;
            }

        existingWalk.Length = walk.Length;
            existingWalk.Name = walk.Name;
            existingWalk.walkDifficultyId = walk.walkDifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
