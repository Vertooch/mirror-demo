using Mirror;

namespace DemoGame
{
    // A simple NetworkMessage to broadcast a player's name and portrait.
    public struct PlayerConnectMessage : NetworkMessage
    {
        public string connectedPlayerName;
        public string connectedPlayerPortraitName;
    }
}
