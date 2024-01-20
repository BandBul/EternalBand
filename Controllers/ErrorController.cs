using EternalBAND.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Controllers;

public class ErrorController : Controller
{
    public ErrorController()
    {
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