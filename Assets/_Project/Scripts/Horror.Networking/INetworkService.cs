using UnityEngine;

namespace Horror.Networking
{
    public interface INetworkService
    {
        AsyncOperation Operation { get; }
        int ConnectedPlayersCount { get; }
        void StartHost();
        void StartClient(string ipAddress);
        void ServerChangeScene(string newSceneName);
    }
}