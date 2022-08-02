using Microsoft.AspNetCore.Mvc;


namespace CarForum.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
