using Mirror;
using UnityEngine;

namespace DemoGame
{
    // This button will allow a client to disconnect from the server or a host to shut down the server
    public class DisconnectButton : MonoBehaviour
    {
        public void Disconnect()
        {
            // If currently hosting, shut down the server
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            // If currently a client, disconnect
            else if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
}
