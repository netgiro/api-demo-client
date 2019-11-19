using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using SampleStoreApp.Models;

namespace SampleStoreApp.Helpers
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
            RestRequest restRequest = GenerateRestRequest(insertCartModel, "/Checkout/InsertCart");
            return ExecuteRequest(restRequest);
        }

        public string CheckCart(string transactionId)
        {
            RestRequest restRequest = GenerateRestRequest(new CheckCartRequest() { TransactionId = transactionId }, "/Checkout/CheckCart");
            return ExecuteRequest(restRequest);
        }

        private RestRequest GenerateRestRequest(object model, string apiAction, string nonce = "nonce")
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

        private string ExecuteRequest(RestRequest restRequest)
        {
            var response = (new RestClient(_apiURL)).Execute(restRequest);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw response.ErrorException;
        }
    }
}
