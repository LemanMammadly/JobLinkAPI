﻿using JobLink.Business.Dtos.AdvertisementDtos;

namespace JobLink.Business.Dtos.CompanyDtos;

public record CompanyInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Logo { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? Website { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<AdvertisementInfoDto> Advertisements { get; set; }
}

