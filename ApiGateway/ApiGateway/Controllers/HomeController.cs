using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "hello";
        }
    }
}
