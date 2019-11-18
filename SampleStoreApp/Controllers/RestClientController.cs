using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using SampleStoreApp.Models;

namespace SampleStoreApp.Controllers
{
    public class RestClientController : Controller
    {
        private readonly IOptions<AppSettings> _appSettings;

        public RestClientController(IOptions<AppSettings> appSettings)
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

            var client = new RestClient(_appSettings.Value.ApiUrl);
            var request = new RestRequest("Checkout/InsertCart", Method.POST, DataFormat.Json);

            var nonce = "nonce";
            var url = client.BaseUrl + request.Resource;
            var jsonModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            var signature = Helpers.CalculateSignature(_appSettings.Value.SecretKey, nonce + url + jsonModel);

            request.AddJsonBody(model);
            request.AddHeader(Helpers.Netgiro_AppKey, _appSettings.Value.ApplicationId);
            request.AddHeader(Helpers.Netgiro_Signature, signature);
            request.AddHeader(Helpers.Netgiro_Nonce, nonce);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true, data = response.Content });
        }

        [HttpPost]
        public ActionResult CheckCart(string transactionId)
        {
            var client = new RestClient(_appSettings.Value.ApiUrl);
            var request = new RestRequest("Checkout/CheckCart", Method.POST, DataFormat.Json);

            var nonce = "nonce";
            var url = client.BaseUrl + request.Resource;
            var model = new CheckCartRequest
            {
                TransactionId = transactionId
            };
            var jsonModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            var signature = Helpers.CalculateSignature(_appSettings.Value.SecretKey, nonce + url + jsonModel);

            request.AddJsonBody(model);
            request.AddHeader(Helpers.Netgiro_AppKey, _appSettings.Value.ApplicationId);
            request.AddHeader(Helpers.Netgiro_Signature, signature);
            request.AddHeader(Helpers.Netgiro_Nonce, nonce);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true, data = response.Content });
        }
    }
}
