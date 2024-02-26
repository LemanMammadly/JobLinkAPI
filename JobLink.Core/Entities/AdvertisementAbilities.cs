using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class AdvertisementAbilities:BaseEntity
{
    public Advertisement Advertisement { get; set; }
    public int AdvertisementId { get; set; }
    public Ability Ability { get; set; }
    public int AbilityId { get; set; }
}


