using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {

        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
        }

        public DbSet<Region> Region { get; set; }
        public DbSet<Walk> walks { get; set; }
        public DbSet<WalkDifficulty> walkDifficulty { get; set; }
    
    }
}
