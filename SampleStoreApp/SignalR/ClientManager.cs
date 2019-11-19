using System.Collections.Generic;

namespace SampleStoreApp.SignalR
{
    public class ClientManager : IClientManager
    {
        // This dictionary contains mappings from SignalR connection id (value) to client transaction id (Netgiro cart id)
        private static Dictionary<string, string> clientMap = new Dictionary<string, string>();

        public void InsertNewClient(string connectionId, string transactionId)
        {
            clientMap.Add(transactionId, connectionId);
        }

        public string GetClientByTransactionId(string transactionId)
        {
            return clientMap.ContainsKey(transactionId) == true ? clientMap[transactionId] : "";
        }
    }
}
