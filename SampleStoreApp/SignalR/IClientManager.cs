namespace SampleStoreApp.SignalR
{
    /// <summary>
    /// This interface is used for mapping SignalR connection id to Netgiro transaction id
    /// </summary>
    public interface IClientManager
    {
        void InsertNewClient(string connectionId, string transactionId);
        string GetClientByTransactionId(string transactionId);
    }
}
