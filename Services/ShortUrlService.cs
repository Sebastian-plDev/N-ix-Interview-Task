namespace MvcProject.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MvcProject.Entities;
using MvcProject.Models.ShortUrls;
using MvcProject.Helpers;

public interface IShortUrlService
{
    Task<IEnumerable<ShortUrl>> GetAllAsync();
    Task<IEnumerable<ShortUrl>> GetNotDeletedAsync();
    Task<ShortUrl> GetByIdAsync(int id);
    Task<string> GetOriginalUrl(string encodedUrl);
    Task UpdateCounter(string encodedUrl);
    Task Create(CreateShortUrl model);
    Task Delete(int id);
}

public class ShortUrlService : IShortUrlService
{
    private EFContext _context;
    private readonly IMapper _mapper;
    public ShortUrlService(
        EFContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShortUrl>> GetAllAsync()
    {
        return await _context.ShortUrls.ToListAsync();
    }

    public async Task<IEnumerable<ShortUrl>> GetNotDeletedAsync()
    {
        return await _context.ShortUrls.Where(x => x.IsDeleted == false).ToListAsync();
    }

    public async Task<int> GetCounterAsync()
    {
        var list = await GetAllAsync();
        return list.Count() + 1;
    }

    public async Task<ShortUrl> GetByIdAsync(int id)
    {
        return await GetShortUrlAsync(id);
    }

    public async Task Create(CreateShortUrl model)
    {
        // map model to new ShortUrl object
        var shortUrl = _mapper.Map<ShortUrl>(model);
        shortUrl.CreatedDate = DateTime.Now;
        shortUrl.Counter = await GetCounterAsync();

        // provide short URL
        shortUrl.ShortendUrl = UrlModifier.Encode(shortUrl.Counter);

        // save user
        await _context.ShortUrls.AddAsync(shortUrl);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ShortUrl shortUrl)
    {
        // update counter
        shortUrl.UsedCount = shortUrl.UsedCount + 1;

        _context.ShortUrls.Update(shortUrl);
        await _context.SaveChangesAsync();
    }

    // instead of Delete I decided to use Update as soft delete method
    public async Task Delete(int id)
    {
        var shortUrl = await GetShortUrlAsync(id);
        // soft delete ShortUrl
        shortUrl.IsDeleted = true;

        _context.ShortUrls.Update(shortUrl);
        _context.SaveChanges();
    }

    private async Task<ShortUrl> GetShortUrlAsync(int id)
    {
        var shortUrl = await _context.ShortUrls.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
        if (shortUrl == null) throw new ApplicationException("Short URL not found");
        return shortUrl;
    }

    public async Task UpdateCounter(string encodedUrl)
    {
        var shortUrl = await GetUrlFromEncodedUrl(encodedUrl);
        await Update(shortUrl);
    }

    private async Task<ShortUrl> GetUrlFromEncodedUrl(string encodedUrl)
    {
        int id = UrlModifier.Decode(encodedUrl);
        return await GetShortUrlAsync(id);
    }

    public async Task<string> GetOriginalUrl(string encodedUrl)
    {
        var shortUrl = await GetUrlFromEncodedUrl(encodedUrl);
        if(shortUrl.OriginalUrl == null) throw new ApplicationException("Something goes wrong - original URL is misssing");
        return shortUrl.OriginalUrl;
    }
}