using EternalBAND.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Controllers;
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : Controller
{
    public ErrorController()
    {
    }

    [HttpGet, Route("hata-olustu/404")]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [HttpGet, Route("hata-olustu/{code}")]
    public IActionResult Index(int code)
    {
        //TODO: loglanacak
        return View(code);
    }
}