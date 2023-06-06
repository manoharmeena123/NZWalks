using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class Walk
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid walkDifficultyId { get; set; }


        // Naviagtion Properties
        public Region Region { get; set; }

        public WalkDifficulty walkDifficulty { get; set; }
    }
}
