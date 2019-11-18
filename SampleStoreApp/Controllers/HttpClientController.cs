using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleStoreApp.Helpers;
using SampleStoreApp.Models;
using System;

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
            model.CallbackUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{Url.Action(nameof(CallbackController.Callback))}";

            INetgiroCart netgiroCart = new NetgiroCartHttpClient(_appSettings.Value.ApiUrl, _appSettings.Value.SecretKey, _appSettings.Value.ApplicationId);

            try
            {
                string response = netgiroCart.InsertCart(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                // Error handling goes here
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult CheckCart(string transactionId)
        {
            INetgiroCart netgiroCart = new NetgiroCartHttpClient(_appSettings.Value.ApiUrl, _appSettings.Value.SecretKey, _appSettings.Value.ApplicationId);

            try
            {
                string response = netgiroCart.CheckCart(transactionId);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                // Error handling goes here
                return Json(new { success = false });
            }
        }
    }
}