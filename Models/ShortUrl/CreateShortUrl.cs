namespace MvcProject.Models.ShortUrls;

using System.ComponentModel.DataAnnotations;
using MvcProject.Entities;

public class CreateShortUrl
{
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings =false,ErrorMessage ="Please enter your URL")]
    [StringLength(maximumLength:256, ErrorMessage ="Your URL exceeds 256 characters")]
    [RegularExpression(pattern: @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?", ErrorMessage ="Invalid URL, ith should start e.g. https://")]
    //[Url(ErrorMessage ="Please enter a valid URL")]
    public string? OriginalUrl { get; set; }
    public DateTime CreatedDate { get; set; }

}