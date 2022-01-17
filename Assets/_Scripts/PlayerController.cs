using UnityEngine;
using Mirror;

namespace DemoGame
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerAvatar))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerAvatar avatar;
        [SerializeField] private Transform cameraContainer;

        void OnValidate()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            characterController.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<NetworkTransform>().clientAuthority = true;
        }

        public override void OnStartLocalPlayer()
        {
            Camera.main.orthographic = false;
            Camera.main.transform.SetParent(cameraContainer != null ? cameraContainer : transform);
            Camera.main.transform.localPosition = new Vector3(0f, 0f, 0f);
            Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            
            avatar.SetNameAndPortrait(PlayerData.playerName, PlayerData.portraitName);
            
            characterController.enabled = true;
        }
    }
}
