using JobLink.Core.Entities.CommonEntities;

namespace JobLink.Core.Entities;

public class Company:BaseEntity
{
    public string Name { get; set; }
    public string? Logo { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? Website { get; set; }
    public DateTime CreateDate { get; set; }
    public ICollection<CompanyIndustry> CompanyIndustries { get; set; }
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
}



