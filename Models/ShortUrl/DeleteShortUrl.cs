namespace MvcProject.Models.ShortUrls;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;
using MvcProject.Entities;

public class DeleteShortUrl
{
    public int Id { get; set; }
    [Display(Name = "Original URL")]
    public string? OriginalUrl { get; set; }
    [Display(Name = "Short URL")]
    public string? ShortendUrl { get; set; }
    [Display(Name = "Used counter")]
    public int UsedCount { get; set; }
    [Display(Name = "Created on")]
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted {get; set;}
}