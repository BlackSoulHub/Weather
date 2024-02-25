using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Weather.Web.DbContext;
using Weather.Web.Entity;
using Weather.Web.Models;
using Weather.Web.Models.Home;
using Weather.Web.Requests;
using Weather.Web.Services;

namespace Weather.Web.Controllers;

public class HomeController(ApplicationDbContext dbContext, DocumentService documentService, IMemoryCache memoryCache) : Controller
{
    private const int RecordsPerPage = 20;
    private const string AvailableYearsCacheKey = "AvailableYears";
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("watch")]
    public async Task<IActionResult> Watch([FromQuery] WatchRequest filter)
    {
        var query = dbContext.Records.AsQueryable();

        if (filter.Year is not null)
        {
            query = query.Where(r => r.DateTime.Year == filter.Year);
        }

        if (filter.Month is not null)
        {
            query = query.Where(r => r.DateTime.Month == filter.Month);
        }

        var queryCount = await query.CountAsync();
        var pageCount = queryCount / RecordsPerPage;
        if (queryCount % 2 != 0)
        {
            pageCount++;
        }
        
        if (filter.Page is not null)
        {
            if (filter.Page < 1)
            {
                return BadRequest("Страница не может быть меньше 1");
            }
            
            var skip = (filter.Page - 1) * RecordsPerPage;
            query = query.Skip(skip.Value);
        }
        
        query = query.Take(RecordsPerPage);
        var result = await query.ToListAsync();
        
        return View(new WatchViewModel
        {
            Filter = filter,
            Items = result,
            AvailableYears = await GetAvailableYearsAsync(),
            PageCount = pageCount
        });
    }

    [HttpGet("upload")]
    public IActionResult UploadPage()
    {
        return View();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        var files = Request.Form.Files;
        if (!files.Any())
        { 
            return BadRequest("Файлы не отправлены");
        }

        var result = new List<WeatherRecord>();
        
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var records = documentService.ParseDocument(stream);
            result.AddRange(records);
        }
        
        await dbContext.Records.AddRangeAsync(result);
        await dbContext.SaveChangesAsync();
        
        return Ok($"Загружено всего {result.Count} записей");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<List<int>> GetAvailableYearsAsync()
    {
        var cacheValue = memoryCache.Get<List<int>>(AvailableYearsCacheKey);
        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var availableYears = await dbContext.Records
            .Select(r => r.DateTime.Year)
            .Distinct()
            .ToListAsync();

        memoryCache.Set(AvailableYearsCacheKey, availableYears, TimeSpan.FromMinutes(5));
        return availableYears;
    }
}