using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using AutoMapper;
using MvcProject.Services;
using MvcProject.Entities;
using MvcProject.Models.ShortUrls;

namespace MvcProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IShortUrlService _shortUrlService;
    private IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, IShortUrlService shortUrlService,
        IMapper mapper)
    {
        _logger = logger;
        _shortUrlService = shortUrlService;
        _mapper = mapper;
    }

    public IActionResult Create()
    {
        return View();
    }

    // POST: Home/Create/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateShortUrl model)
    {
        if(ModelState.IsValid)
        {
            await _shortUrlService.Create(model);
            _logger.LogInformation("Succesfully created short Url for original Url: {orgUrl} !", model.OriginalUrl);
            return RedirectToAction(nameof(Dashboard));
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Dashboard()
    {
        IEnumerable<ShortUrl> shortUrls = await _shortUrlService.GetNotDeletedAsync();
        IEnumerable<DeleteShortUrl> myShortUrls  = _mapper.Map<IEnumerable<ShortUrl>, IEnumerable<DeleteShortUrl>>(shortUrls);
        _logger.LogInformation("You have: {count} URL to show!", myShortUrls.Count());
        return View(myShortUrls);
    }

    // GET: Home/Delete/1
    public async Task<IActionResult> Delete(int? shortUrlId)
    {
        if (shortUrlId == null)
        {
            return NotFound();
        }
        var shortUrlToDelete = await _shortUrlService.GetByIdAsync(shortUrlId.Value);
        DeleteShortUrl sUrlToDelete = _mapper.Map<DeleteShortUrl>(shortUrlToDelete);

        return View(sUrlToDelete);
    }

    // POST: Home/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int shortUrlId)
    {
        await _shortUrlService.Delete(shortUrlId);
        _logger.LogInformation("Your short Url is succesfully deleted id: {id} !", shortUrlId);

        return RedirectToAction(nameof(Dashboard));
    }

    // GET: /a
    [Route("{encodedUrl}")]
    [HttpGet]
    public async Task<IActionResult> Update(string encodedUrl)
    {
        try
        {
            await _shortUrlService.UpdateCounter(encodedUrl);
            string originalUrl = await _shortUrlService.GetOriginalUrl(encodedUrl);
            _logger.LogInformation("Succesfully visited shortened URL - original URL: {orgUrl}, short URL: https://localhost/{encoded} !", originalUrl, encodedUrl);
            return Redirect(originalUrl);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
