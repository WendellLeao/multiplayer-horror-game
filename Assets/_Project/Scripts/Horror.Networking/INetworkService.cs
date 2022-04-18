namespace Horror.Networking
{
    public interface INetworkService
    {
        void StartHost();
        void StartClient(string ipAddress);
        void ServerChangeScene(string newSceneName);
    }
}