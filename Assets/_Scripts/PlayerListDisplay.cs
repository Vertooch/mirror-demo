using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace DemoGame
{
    // The PlayerListDisplay shows the names of all players currently in the room.
    // It reacts to the OnPlayerConnected and OnPlayerDisconnected actions from CustomNetworkManager
    // to add a player's name when they connect or remove a player's name when they disconnect.
    public class PlayerListDisplay : NetworkBehaviour
    {
        [SerializeField] private Text listText;

        [SyncVar(hook = nameof(UpdateDisplay))] public string listString;

        private List<string> playerList = new List<string>();

        public void AddPlayer(string name)
        {
            playerList.Add(name);
            UpdateListString();
        }

        public void RemovePlayer(string name)
        {
            playerList.Remove(name);
            UpdateListString();
        }

        private void UpdateListString()
        {
            // Construct a new string of all active player's names then update listString with this new string
            string updatedListString = "";

            for (int i = 0; i < playerList.Count; i++)
            {
                if (i > 0) updatedListString += "\n";
                updatedListString += playerList[i];
            }

            listString = updatedListString;
        }

        private void UpdateDisplay(string oldList, string newList)
        {
            listText.text = newList;
        }
    }
}
