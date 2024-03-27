using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class Reqruiment:BaseEntity
{
    public string Text { get; set; }
    public Advertisement Advertisement { get; set; }
    public int AdvertisementId { get; set; }
}

