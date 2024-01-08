using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class EmailToken:BaseEntity
{
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public DateTime SendDate { get; set; }
    public string Token { get; set; }
}



