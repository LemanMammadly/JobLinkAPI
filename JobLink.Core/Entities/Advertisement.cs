using JobLink.Core.Entities.CommonEntities;
using JobLink.Core.Enums;

namespace JobLink.Core.Entities;

public class Advertisement:BaseEntity
{
    public string Title { get; set; }
    public string City { get; set; }
    public decimal? Salary { get; set; }
    public string WorkGraphic { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Ability { get; set; }
    public string JobDesc { get; set; }
    public string Reqruiment { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public int ViewCount { get; set; }
    public AdvertisementStatus Status { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
}





