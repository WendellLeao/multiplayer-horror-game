namespace Horror.Networking
{
    public interface INetworkService
    {
        int ConnectedPlayersCount { get; }
        void StartHost();
        void StartClient(string ipAddress);
        void ServerChangeScene(string newSceneName);
    }
}