using Microsoft.AspNetCore.Mvc;

namespace MenuMapper.Controllers
{
    public class HomeController : Controller
    {
        // This simply loads your Index.cshtml page containing the React app.
        // It no longer needs the AppDbContext or complex LINQ queries.
        public IActionResult Index()
        {
            return View();
        }
    }
}