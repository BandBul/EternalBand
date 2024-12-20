using EternalBAND.Common;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Controllers;
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : Controller
{
    public ErrorController()
    {
    }

    [HttpGet, Route(EndpointConstants.PageNotFound)]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [HttpGet, Route(EndpointConstants.ErrorRouteWithCode)]
    public IActionResult Index(int code)
    {
        //TODO: loglanacak
        return View(code);
    }
}