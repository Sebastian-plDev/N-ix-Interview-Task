namespace MvcProject.Helpers;

using AutoMapper;
using MvcProject.Entities;
using MvcProject.Models.ShortUrls;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateShortUrl <-> ShortUrl
        CreateMap<CreateShortUrl, ShortUrl>().ReverseMap();

        // DeleteShortUrl <-> ShortUrl
        CreateMap<DeleteShortUrl, ShortUrl>().ReverseMap()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));
    }
}