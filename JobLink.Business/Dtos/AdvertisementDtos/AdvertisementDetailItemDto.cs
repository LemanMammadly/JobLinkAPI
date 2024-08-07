﻿using JobLink.Business.Dtos.CategoryDtos;
using JobLink.Business.Dtos.CompanyDtos;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Business.Dtos.ReqruimentDtos;
using JobLink.Core.Enums;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record AdvertisementDetailItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string City { get; set; }
    public decimal? Salary { get; set; }
    public string WorkGraphic { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Ability { get; set; }
    public string Reqruiment { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public int ViewCount { get; set; }
    public string Status { get; set; }
    public string State { get; set; }
    public int CategoryId { get; set; }
    public bool IsDeleted { get; set; }
    public CategoryInfoDto Category { get; set; }
    public CompanyMiniInfoDto Company { get; set; }
    public IEnumerable<AdvertisementAbilityDto> AdvertisementAbilities { get; set; }
    public IEnumerable<JobDescriptionListItemDto> JobDescriptions { get; set; }
    public IEnumerable<ReqruimentListItemDto> Reqruiments { get; set; }
    public CountAdverDto AdvertisementCount { get; set; }
}


