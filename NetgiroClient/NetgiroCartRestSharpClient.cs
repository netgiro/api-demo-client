﻿using System.Threading.Tasks;
using NetgiroClient.Helpers;
using NetgiroClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace NetgiroClient
{
    public class NetgiroCartRestSharpClient : INetgiroCart
    {
        private string _apiURL;
        private string _secretKey;
        private string _applicationId;

        public NetgiroCartRestSharpClient(string apiURL, string secretKey, string applicationId)
        {
            _apiURL = apiURL;
            _secretKey = secretKey;
            _applicationId = applicationId;
        }

        public string InsertCart(InsertCartModel insertCartModel)
        {
            Task<string> insertCartTask = InsertCartAsync(insertCartModel);
            insertCartTask.Wait();

            return insertCartTask.Result;
        }

        public Task<string> InsertCartAsync(InsertCartModel insertCartModel)
        {
            RestRequest restRequest = GenerateRestRequest(insertCartModel, Constants.Netgiro_Api_InsertCartURL, RandomString.Generate());
            return ExecuteRequest(restRequest);
        }

        public string CheckCart(string transactionId)
        {
            Task<string> checkCartTask = CheckCartAsync(transactionId);
            checkCartTask.Wait();

            return checkCartTask.Result;
        }

        public Task<string> CheckCartAsync(string transactionId)
        {
            RestRequest restRequest = GenerateRestRequest(new CheckCartRequestModel() { TransactionId = transactionId }, Constants.Netgiro_Api_CheckCartURL, RandomString.Generate());
            return ExecuteRequest(restRequest);
        }

        public bool CancelCart(string transactionId)
        {
            Task<bool> cancelCartTask = CancelCartAsync(transactionId);
            cancelCartTask.Wait();

            return cancelCartTask.Result;
        }

        public Task<bool> CancelCartAsync(string transactionId)
        {
            RestRequest restRequest = GenerateRestRequest(new CancelCartRequestModel() { TransactionId = transactionId }, Constants.Netgiro_Api_CancelCartURL, RandomString.Generate());
            return ExecuteRequestBool(restRequest);
        }

        public bool ConfirmCart(string transactionId, bool accept)
        {
            Task<bool> cancelCartTask = ConfirmCartAsync(transactionId, accept);
            cancelCartTask.Wait();

            return cancelCartTask.Result;
        }

        public Task<bool> ConfirmCartAsync(string transactionId, bool accept)
        {
            var identifier = accept ? "1" : "0";
            RestRequest restRequest = GenerateRestRequest(new ConfirmCartRequestModel() { TransactionId = transactionId, Identifier = identifier }, Constants.Netgiro_Api_ConfirmCartURL, RandomString.Generate());
            return ExecuteRequestBool(restRequest);
        }

        private RestRequest GenerateRestRequest(object model, string apiAction, string nonce)
        {
            var request = new RestRequest(apiAction, Method.POST, DataFormat.Json);

            // we don't know if an URL in config has a slash at the end of it, so we remove it from URL, and always include it at the start of path
            var url = _apiURL.TrimEnd('/') + request.Resource;
            var jsonModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            var signature = Signature.CalculateSignature(_secretKey, nonce + url + jsonModel);

            request.AddJsonBody(model);
            request.AddHeader(Constants.Netgiro_AppKey, _applicationId);
            request.AddHeader(Constants.Netgiro_Signature, signature);
            request.AddHeader(Constants.Netgiro_Nonce, nonce);

            return request;
        }

        private async Task<string> ExecuteRequest(RestRequest restRequest)
        {
            IRestResponse response = await (new RestClient(_apiURL)).ExecuteTaskAsync(restRequest);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw response.ErrorException;
        }

        private async Task<bool> ExecuteRequestBool(RestRequest restRequest)
        {
            IRestResponse response = await (new RestClient(_apiURL)).ExecuteTaskAsync(restRequest);

            return response.IsSuccessful;
        }
    }
}
