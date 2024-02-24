using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Weather.Web.Models;
using Weather.Web.Services;

namespace Weather.Web.Controllers;

public class HomeController(DocumentService documentService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestDocs()
    {
        var path = Path.Combine("DOCS", "moskva_2010.xlsx");
        await using var file = System.IO.File.Open(path, FileMode.Open);
        var data = documentService.ParseDocument(file);
        return Ok(data);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}