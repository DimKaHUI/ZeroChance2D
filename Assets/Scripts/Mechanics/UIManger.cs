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
        private GameObject cameraObject;
        private Vector3 prevOffset;
        private Item itemUnderCursor;

        // Use this for initialization
        void Start()
        {
            cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            if(cameraObject == null)
                Debug.LogError("Camera not found!");
            prevOffset = Vector3.zero;
        }


        void FixedUpdate()
        {
            #region Movement and camera controlling
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
                    Cursor.lockState = CursorLockMode.None;
                    prevOffset = Vector3.zero;
                }
                else
                {
                    CurrentMode = ControllMode.Shooting;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
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

                Ray ray = cameraObject.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                var point = ray.GetPoint(CameraRelativePosition.z);
                
                float angle = Mathf.Atan2(point.y - pos.y, point.x - pos.x);
                rig.gameObject.transform.rotation =
                    Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90);

            }

            if (CurrentMode == ControllMode.Interaction)
                cameraObject.transform.position = pos +
                                            CameraRelativePosition;
            if (CurrentMode == ControllMode.Shooting)
            {
                float move_x = Input.GetAxis("Mouse X");
                float move_y = Input.GetAxis("Mouse Y");
                prevOffset += new Vector3(move_x, move_y, 0) * AimSensitivity;
                cameraObject.transform.position = new Vector3(pos.x, pos.y, CameraRelativePosition.z) + prevOffset;
            }
#endregion

            #region Item picking

            if (CurrentMode == ControllMode.Interaction)
            {
                RaycastHit2D hitItem;

                hitItem = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, 0),
                    Mathf.Infinity);
                if (hitItem.collider == null)
                    itemUnderCursor = null;
                else
                {
                    itemUnderCursor = hitItem.collider.gameObject.GetComponent<Item>();
                    Debug.Log(itemUnderCursor.ItemName);
                }
            }

            

            #endregion


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