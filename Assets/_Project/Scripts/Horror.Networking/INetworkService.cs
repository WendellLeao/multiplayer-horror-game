using Mirror;

namespace Horror.Networking
{
    public interface INetworkService
    {
        NetworkConnectionToClient PlayerConn { get; }
        void StartHost();
        void StartClient(string ipAddress);
    }
}