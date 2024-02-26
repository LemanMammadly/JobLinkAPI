using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class Ability:BaseEntity
{
    public string Name { get; set; }
    public ICollection<AdvertisementAbilities>? AdvertisementAbilities { get; set; }
}




