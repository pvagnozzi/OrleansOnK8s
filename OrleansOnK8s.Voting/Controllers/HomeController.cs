using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrleansOnK8s.Voting.Models;

namespace OrleansOnK8s.Voting.Controllers;

[Route("")]
[Route("Home")]
[Route("Home/Index")]
public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger _logger = logger;

    public ActionResult Index()
    {
        _logger.LogInformation("Returning Index page");
        return View();
    }

    [Route("Home/Error")]
    public ActionResult Error()
    {
        _logger.LogInformation("Returning Error page");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
