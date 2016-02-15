namespace Neo4jDotNetDemo.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return new FilePathResult("Views/home/index.html", "text/html");
        }
    }
}