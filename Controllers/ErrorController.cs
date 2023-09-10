using EternalBAND.Data;
using EternalBAND.Models;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Controllers;

public class ErrorController : Controller
{
    private ApplicationDbContext _context;

    public ErrorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Route("hata-olustu/404")]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [Route("hata-olustu/{code}")]
    public IActionResult Index(int code)
    {
        
        //TODO: loglanacak
        return View();
    }
}