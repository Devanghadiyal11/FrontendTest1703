using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontendtest1703.Controllers
{
    [Authorize]
    public class QueueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
