using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace DemoGame
{
    // MainMenu captures the player's chosen name and portrait so that the data can be broadcast to 
    // all client's after the player connects to the server. It also prevents player's from attempting
    // to create/join a room before entering a player name.
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Image portraitImage;
        [SerializeField] private AlertMessage alertPrefab;

        public void StartOrJoinGame()
        {
            if (string.IsNullOrEmpty(nameText.text))
            {
                ShowAlertWithMessage("You must enter a name before starting or joining a game!");
                return;
            }

            PlayerData.playerName = nameText.text;
            PlayerData.portraitName = portraitImage.sprite.name;

            // To promote ease-of-use, I chose to use a single-button approach that combines creating and joining
            // a server. Since this project is limited to LAN, there can only be a single server at any given time.
            // When a player presses the button to create/join a room, we first attempt to start a new room as the host.
            // If the network socket is already in use, then we simply join the existing room as a client.
            try
            {
                NetworkManager.singleton.StartHost();
            }
            catch (SocketException exception)
            {
                NetworkManager.singleton.StartClient();
            }
        }

        private void ShowAlertWithMessage(string message)
        {
            AlertMessage alert = Instantiate(alertPrefab, transform.parent, false);
            alert.SetMessageText(message);
        }
    }
}
