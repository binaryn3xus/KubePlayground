using Microsoft.AspNetCore.Mvc;

namespace KubePlayground.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult SignalRTest() => View();
    public IActionResult JobHome() => View();
}
