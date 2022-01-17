using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DemoGame
{
    // PortraitSelector allows a player to select their player portrait which will be displayed on their
    // avatar while in a room with other players.
    public class PortraitSelector : MonoBehaviour
    {
        [SerializeField] private Image portraitImage;
        [SerializeField] private Sprite[] portraits;

        private int portraitIndex;
        private RectTransform portraitTransform;

        private void Start()
        {
            portraitTransform = portraitImage.gameObject.GetComponent<RectTransform>();
            
            // Start with a random portrait
            portraitIndex = Random.Range(0, portraits.Length);
            SetPortrait();
        }

        public void NextPortrait()
        {
            portraitIndex = (portraitIndex + 1) % portraits.Length;
            SetPortrait();
        }

        public void PreviousPortrait()
        {
            portraitIndex = portraitIndex > 0 ? portraitIndex - 1 : portraits.Length - 1;
            SetPortrait();
        }

        private void SetPortrait()
        {
            portraitImage.sprite = portraits[portraitIndex];

            // Since the portraits are slightly different sizes we call SetNativeSize to avoid distortion.
            portraitImage.SetNativeSize();
            
            // We must also adjust the portrait transform's pivot to align with the pivot of the portrait sprite
            Vector2 size = portraitTransform.sizeDelta;
            size *= portraitImage.pixelsPerUnit;
            Vector2 pixelPivot = portraitImage.sprite.pivot;
            Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            portraitTransform.pivot = percentPivot;
        }
    }
}
