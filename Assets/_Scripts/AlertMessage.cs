using UnityEngine;
using UnityEngine.UI;

namespace DemoGame
{
    // A multi-purpose UI alert that displays a provided messages and
    // destroys itself when the close button is pressed.
    // In this demo AlertMessage is only used if the player attempts
    // to create/join a room before entering a player name.
    public class AlertMessage : MonoBehaviour
    {
        [SerializeField] private Text messageText;

        public void SetMessageText(string message)
        {
            messageText.text = message;
        }

        public void DismissAlert()
        {
            Destroy(gameObject);
        }
    }
}
