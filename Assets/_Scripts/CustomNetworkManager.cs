using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

namespace DemoGame
{
    // A customized subclass of Mirror's NetworkManager to add functionality for this demo.
    public class CustomNetworkManager : NetworkManager
    {
        // playerDictionary keeps a record of each connected player's connection Id and name.
        // When a player disconnects, we use this dictionary to determine the name of the player
        // that disconnected so that the PlayerListDisplay can be correctly updated.
        private Dictionary<int, string> playerDictionary = new Dictionary<int, string>();

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<PlayerConnectMessage>(InitializeNewPlayer);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            
            // When a client connects to the server, we broadcast a message including that player's name
            // and portrait so that the player's avatar can be initialized with their name and portrait.
            PlayerConnectMessage newMessage = new PlayerConnectMessage
            {
                connectedPlayerName = PlayerData.playerName,
                connectedPlayerPortraitName = PlayerData.portraitName
            };

            conn.Send(newMessage);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (playerDictionary.ContainsKey(conn.connectionId))
            {
                // Update the PlayerListDisplay to remove the disconnected player
                FindObjectOfType<PlayerListDisplay>().RemovePlayer(playerDictionary[conn.connectionId]);

                // Remove the disconnected player from playerDictionary so that a new player can connect 
                // with the now available connection Id.
                playerDictionary.Remove(conn.connectionId);
            }

            base.OnServerDisconnect(conn);
        }

        // Custom player initialization to update the player avatar with their name and portrait.
        private void InitializeNewPlayer(NetworkConnection conn, PlayerConnectMessage message)
        {
            Transform startPos = GetStartPosition();
            GameObject newPlayerGO = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);

            // Give the GameObject a more useful name
            newPlayerGO.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            
            // Set this GameObject as the primary controller
            NetworkServer.AddPlayerForConnection(conn, newPlayerGO);

            // Update the player's avatar with their name and portrait
            PlayerAvatar avatar = newPlayerGO.GetComponent<PlayerAvatar>();
            if (avatar != null)
            {
                avatar.characterName = message.connectedPlayerName;
                avatar.portraitName = message.connectedPlayerPortraitName;
            }

            if (!playerDictionary.ContainsKey(conn.connectionId))
            {
                // Add the newly connected player to playerDictionary so that their name can be removed from the
                // PlayerListDisplay when/if they disconnect.
                playerDictionary.Add(conn.connectionId, message.connectedPlayerName);
                
                // Update the PlayerListDisplay to add the connected player
                FindObjectOfType<PlayerListDisplay>().AddPlayer(playerDictionary[conn.connectionId]);
            }
        }
    }
}
