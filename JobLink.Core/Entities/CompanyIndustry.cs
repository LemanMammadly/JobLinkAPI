using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class CompanyIndustry:BaseEntity
{
    public Company Company { get; set; }
    public int CompanyId { get; set; }
    public Industry Industry { get; set; }
    public int IndustryId { get; set; }
}


