﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetgiroClient;
using NetgiroClient.Models;

namespace SampleStoreApp.Controllers
{
    public class RestSharpClientController : Controller
    {
        private readonly AppSettings _appSettings;

        public RestSharpClientController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            var model = new InsertCartModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCart(InsertCartModel model)
        {
            model.CallbackUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{Url.Action(nameof(CallbackController.Callback), "Callback")}";

            INetgiroCart netgiroCart = new NetgiroCartRestSharpClient(_appSettings.ApiUrl, _appSettings.SecretKey, _appSettings.ApplicationId);

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
            INetgiroCart netgiroCart = new NetgiroCartRestSharpClient(_appSettings.ApiUrl, _appSettings.SecretKey, _appSettings.ApplicationId);

            try
            {
                var response = await netgiroCart.CheckCartAsync(transactionId);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelCart(string transactionId)
        {
            INetgiroCart netgiroCart = new NetgiroCartRestSharpClient(_appSettings.ApiUrl, _appSettings.SecretKey, _appSettings.ApplicationId);

            try
            {
                var response = await netgiroCart.CancelCartAsync(transactionId);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCart(string transactionId, bool confirm)
        {
            INetgiroCart netgiroCart = new NetgiroCartRestSharpClient(_appSettings.ApiUrl, _appSettings.SecretKey, _appSettings.ApplicationId);

            try
            {
                var response = await netgiroCart.ConfirmCartAsync(transactionId, confirm);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }
    }
}
