using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleStoreApp.Helpers;
using SampleStoreApp.Models;

namespace SampleStoreApp.Controllers
{
    public class HttpClientController : Controller
    {
        private readonly IOptions<AppSettings> _appSettings;

        public HttpClientController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult Index()
        {
            var model = new InsertCartModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult InsertCart(InsertCartModel model)
        {
            return null;
        }

        [HttpPost]
        public ActionResult CheckCart(string transactionId)
        {
            return null;
        }
    }
}