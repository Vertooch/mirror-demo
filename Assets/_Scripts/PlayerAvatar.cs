using System;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

namespace DemoGame
{
    // PlayerAvatar handles the visual display of the player GameObject including the name, portrait, and color.
    public class PlayerAvatar : NetworkBehaviour
    {
        [SerializeField] private TextMesh nameText;
        [SerializeField] private SpriteRenderer portraitRenderer;
        [SerializeField] private Sprite[] portraits;
        
        [SyncVar(hook = nameof(SetName))] public string characterName;
        [SyncVar(hook = nameof(SetPortrait))] public string portraitName;
        [SyncVar(hook = nameof(SetColor))] public Color32 color = Color.black;
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        // Unity clones the material when GetComponent<Renderer>().material is called
        // Cache it here and destroy it in OnDestroy to prevent a memory leak
        Material cachedMaterial;

        void SetColor(Color32 _, Color32 newColor)
        {
            if (cachedMaterial == null) cachedMaterial = GetComponentInChildren<Renderer>().material;
            cachedMaterial.color = newColor;
        }
        
        private void SetName(string oldName, string newName)
        {
            nameText.text = newName;
        }

        private void SetPortrait(string oldPortrait, string newPortrait)
        {
            portraitRenderer.sprite = Array.Find(portraits, sprite => sprite.name.Equals(newPortrait));
        }

        public void SetNameAndPortrait(string characterName, string portraitName)
        {
            nameText.text = characterName;
            portraitRenderer.sprite = Array.Find(portraits, sprite => sprite.name.Equals(portraitName));
        }

        void OnDestroy()
        {
            Destroy(cachedMaterial);
        }
    }
}
