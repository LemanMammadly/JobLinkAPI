using JobLink.Business.Dtos.CompanyDtos;

namespace JobLink.Business.Dtos.IndustryDtos;

public class IndustryListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<CompanyIndustryDto> CompanyIndustries { get; set; }
}



