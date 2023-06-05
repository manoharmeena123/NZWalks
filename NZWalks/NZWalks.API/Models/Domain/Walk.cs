namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
          public Guid Id { get; set; }
          public string Name { get; set; }
          public double Length { get; set; }
          public Guid RegionId { get; set; }
          public Guid WalkDifficulty { get; set; } 

        // Naviagtion Properties
         public Region Region { get; set; } 

        public WalkDifficulty walkDifficulty { get; set; }
    }
}
