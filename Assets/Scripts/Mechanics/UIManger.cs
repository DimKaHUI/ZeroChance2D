using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ZeroChance2D
{

    public enum HandSide { Left, Right}
    public enum ControllMode { Interaction, Shooting}
    public class UIManger : NetworkBehaviour
    {
        public Image LeftHandImage;
        public Image RightHandImage;
        public Image HealthIndicator;
        public HandSide ActiveHand;
        public ControllMode CurrentMode;
        private PlayerController attachedPlayerController;

        private Rigidbody2D rig;

        // Use this for initialization
        void Start()
        {

        }

        
        void FixedUpdate()
        {
            if (attachedPlayerController == null)
            {
                Debug.Log(attachedPlayerController);
                return;
            }

            if (CurrentMode == ControllMode.Interaction)
            {
                float forward = Input.GetAxis("Vertical");
                float turn = Input.GetAxis("Horizontal");
                rig.velocity = rig.gameObject.transform.up * attachedPlayerController.WalkSpeed * forward;
                rig.gameObject.transform.rotation = Quaternion.Euler(0, 0, rig.gameObject.transform.rotation.eulerAngles.z);
                rig.angularVelocity = -turn * attachedPlayerController.RotationSpeed;
            }
        }

        public PlayerController AttachedController
        {
            get { return attachedPlayerController; }
            set
            {
                attachedPlayerController = value;
                rig = value.gameObject.GetComponent<Rigidbody2D>();
            }
        }

    
    }
}