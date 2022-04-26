using UnityEngine;

namespace Horror.Networking
{
    public interface INetworkService
    {
        AsyncOperation Operation { get; }
        int ConnectedPlayersCount { get; }
        void StartHost();
        void StartClient(string ipAddress);
        void StopHost();
        void StopClient();
        void ServerChangeScene(string newSceneName);
    }
}