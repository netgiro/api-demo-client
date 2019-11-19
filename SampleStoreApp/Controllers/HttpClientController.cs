using Microsoft.AspNetCore.Mvc;
using SampleStoreApp.Helpers;
using SampleStoreApp.Models;
using System;
using System.Threading.Tasks;

namespace SampleStoreApp.Controllers
{
    public class HttpClientController : Controller
    {
        private readonly AppSettings _appSettings;

        public HttpClientController(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult Index()
        {
            var model = new InsertCartModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCart(InsertCartModel model)
        {
            model.CallbackUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{Url.Action(nameof(CallbackController.Callback))}";

            INetgiroCart netgiroCart = new NetgiroCartHttpClient(_appSettings.ApiUrl, _appSettings.SecretKey, _appSettings.ApplicationId);

            try
            {
                string response = await netgiroCart.InsertCartAsync(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckCart(string transactionId)
        {
            INetgiroCart netgiroCart = new NetgiroCartHttpClient(_appSettings.ApiUrl, _appSettings.SecretKey, _appSettings.ApplicationId);

            try
            {
                string response = await netgiroCart.CheckCartAsync(transactionId);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }
    }
}