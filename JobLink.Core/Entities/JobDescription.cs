using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class JobDescription:BaseEntity
{
    public string Description { get; set; }
    public Advertisement Advertisement { get; set; }
    public int AdvertisementId { get; set; }
}





