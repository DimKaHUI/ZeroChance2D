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
        public Vector3 CameraRelativePosition;
        public float CameraVelocityOffset;
        public float AimSensitivity;

        private Rigidbody2D rig;
        private GameObject camera;
        private Vector3 prevMousePos;

        // Use this for initialization
        void Start()
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
            if(camera == null)
                Debug.LogError("Camera not found!");
            prevMousePos = Input.mousePosition;
        }


        void FixedUpdate()
        {
            Vector3 pos = attachedPlayerController.gameObject.transform.position;

            if (attachedPlayerController == null)
            {
                Debug.Log(attachedPlayerController);
                return;
            }

            if (Input.GetButtonDown("Change Mode"))
            {
                if (CurrentMode == ControllMode.Shooting)
                {
                    CurrentMode = ControllMode.Interaction;
                    Cursor.visible = true;
                }
                else
                {
                    CurrentMode = ControllMode.Shooting;
                    Cursor.visible = false;
                }
                Debug.Log("Mode changed");
            }

            if (CurrentMode == ControllMode.Interaction)
            {
                float forward = Input.GetAxis("Vertical");
                float turn = Input.GetAxis("Horizontal");
                rig.velocity = rig.gameObject.transform.up * attachedPlayerController.WalkSpeed * forward;
                rig.gameObject.transform.rotation =
                    Quaternion.Euler(0, 0, rig.gameObject.transform.rotation.eulerAngles.z);
                rig.angularVelocity = -turn * attachedPlayerController.RotationSpeed;
            }
            if (CurrentMode == ControllMode.Shooting)
            {
                float forward = Input.GetAxis("Vertical");
                float turn = Input.GetAxis("Horizontal");
                rig.velocity = rig.gameObject.transform.up * attachedPlayerController.WalkSpeed * forward;
                rig.gameObject.transform.rotation =
                    Quaternion.Euler(0, 0, rig.gameObject.transform.rotation.eulerAngles.z);
                rig.angularVelocity = -turn * attachedPlayerController.RotationSpeed;
            }

            if (CurrentMode == ControllMode.Interaction)
                camera.transform.position = pos +
                                            CameraRelativePosition;
            if (CurrentMode == ControllMode.Shooting)
            {

                camera.transform.position = new Vector3(pos.x, pos.y, CameraRelativePosition.z) + (Input.mousePosition - prevMousePos) * AimSensitivity;
            }

            prevMousePos = Input.mousePosition;


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