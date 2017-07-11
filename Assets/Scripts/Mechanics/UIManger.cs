using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ZeroChance2D
{
    [Serializable]
    public enum HandSide { Left, Right}
    public enum ControllMode { Interaction, Shooting}
    public class UIManger : NetworkBehaviour
    {
        public NetworkManager NetManager;
        public Image LeftHandImage;
        public Image RightHandImage;
        public RawImage LeftItemImage;
        public RawImage RightItemImage;
        public Image HealthIndicator;
        public HandSide ActiveHand;
        public ControllMode CurrentMode;
        private PlayerController Controller;
        public Vector3 CameraRelativePosition;
        public float CameraVelocityOffset;
        public float AimSensitivity;
        public float ItemPickRange = 30f;

        private Rigidbody2D rig;
        private GameObject cameraObject;
        private Vector3 prevOffset;

        private Item itemUnderCursor;
        private GameObject movableUnderCursor;
        private GameObject livingUnderCursor;

        

        // Use this for initialization
        void Start()
        {
            cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            if(cameraObject == null)
                Debug.LogError("Camera not found!");
            prevOffset = Vector3.zero;

            if(NetManager == null)
                Debug.LogError("No network manager attached!");
        }


        void FixedUpdate()
        {
#region Movement and camera controlling
            Vector3 pos = Controller.gameObject.transform.position;
            

            if (Controller == null)
            {
                Debug.Log(Controller);
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
                rig.velocity = rig.gameObject.transform.up * Controller.WalkSpeed * forward;
                rig.gameObject.transform.rotation =
                    Quaternion.Euler(0, 0, rig.gameObject.transform.rotation.eulerAngles.z);
                rig.angularVelocity = -turn * Controller.RotationSpeed;
            }
            if (CurrentMode == ControllMode.Shooting)
            {
                float forward = Input.GetAxis("Vertical");
                float turn = Input.GetAxis("Horizontal");
                rig.velocity = rig.gameObject.transform.up * Controller.WalkSpeed * forward;

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

#region Finding objects under cursor
            var clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPoint.z = Controller.gameObject.transform.position.z;

            bool hitUI = RaycastWorldUI();

            var hitItem = Physics2D.Raycast(clickPoint, new Vector2(0, 0), Mathf.Infinity, LayerMask.NameToLayer("Items"));
            itemUnderCursor = hitItem.collider == null ? null : hitItem.collider.gameObject.GetComponent<Item>();

            var hitMovable = Physics2D.Raycast(clickPoint, new Vector2(0, 0), Mathf.Infinity, LayerMask.NameToLayer("Movables"));
            movableUnderCursor = hitMovable.collider == null ? null : hitItem.collider.gameObject;

            var hitLiving = Physics2D.Raycast(clickPoint, new Vector2(0, 0), Mathf.Infinity, LayerMask.NameToLayer("Livings"));
            livingUnderCursor = hitLiving.collider == null ? null : hitItem.collider.gameObject;

            #endregion

#region Item picking

            if (CurrentMode == ControllMode.Interaction)
            {
                

                if (Input.GetMouseButtonDown(0) && !hitUI)
                {
                    if (itemUnderCursor != null && Vector2.Distance(Controller.transform.position, itemUnderCursor.gameObject.transform.position) <= ItemPickRange) // If cursor is upon an item
                    {
                        switch (ActiveHand)
                        {
                            case HandSide.Right:
                                if (Controller.Equipment.RightHandItem == null)
                                {
                                    itemUnderCursor.gameObject.SetActive(false);
                                    Controller.Equipment.RightHandItem = itemUnderCursor;
                                }
                                break;
                            case HandSide.Left:
                                if (Controller.Equipment.LeftHandItem == null)
                                {
                                    itemUnderCursor.gameObject.SetActive(false);
                                    Controller.Equipment.LeftHandItem = itemUnderCursor;
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else if( Vector2.Distance(Controller.transform.position, clickPoint) <= ItemPickRange)
                    {
                        switch (ActiveHand)
                        {
                            case HandSide.Left:
                                if (Controller.Equipment.LeftHandItem != null)
                                {
                                    Controller.Equipment.LeftHandItem.gameObject.SetActive(true);
                                    Controller.Equipment.LeftHandItem.gameObject.transform.position = clickPoint;
                                    Controller.Equipment.LeftHandItem = null;
                                }
                                break;
                            case HandSide.Right:
                                if (Controller.Equipment.RightHandItem != null)
                                {
                                    Controller.Equipment.RightHandItem.gameObject.SetActive(true);
                                    Controller.Equipment.RightHandItem.gameObject.transform.position = clickPoint;

                                    Controller.Equipment.RightHandItem = null;
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

            }
            #endregion


        }

        void Update()
        {
            // Drawing sprites
            if (Controller != null && Controller.Equipment != null)
            {
                if (Controller.Equipment.LeftHandItem != null)
                {
                   // TODO
                    var image = Controller.Equipment.LeftHandItem.gameObject.GetComponent<SpriteRenderer>()
                        .sprite.texture;
                    var imageSize = new Vector2(image.width, image.height);

                    if (imageSize.x > imageSize.y)
                    {
                        float coef = imageSize.y / imageSize.x;
                        LeftItemImage.rectTransform.sizeDelta = new Vector2(55, 55 * coef);
                    }
                    else
                    {
                        float coef = imageSize.x / imageSize.y;
                        LeftItemImage.rectTransform.sizeDelta = new Vector2(55 * coef, 55);
                    }

                    LeftItemImage.texture = image;
                }
                else
                    LeftItemImage.texture = null;


                if (Controller.Equipment.RightHandItem != null)
                {
                    var image = Controller.Equipment.RightHandItem.gameObject.GetComponent<SpriteRenderer>()
                        .sprite.texture;
                    var imageSize = new Vector2(image.width, image.height);

                    if (imageSize.x > imageSize.y)
                    {
                        float coef = imageSize.y / imageSize.x;
                        RightItemImage.rectTransform.sizeDelta = new Vector2(55, 55 * coef);
                    }
                    else
                    {
                        float coef = imageSize.x / imageSize.y;
                        RightItemImage.rectTransform.sizeDelta = new Vector2(55 * coef, 55);
                    }

                    RightItemImage.texture = image;
                   
                }
                else
                    RightItemImage.texture = null;


            }
        }


        public PlayerController AttachedController
        {
            get { return Controller; }
            set
            {
                Controller = value;
                rig = value.gameObject.GetComponent<Rigidbody2D>();
            }
        }

        bool RaycastWorldUI()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                foreach (var res in results)
                {
                    if (res.gameObject.layer == LayerMask.NameToLayer("UI"))
                        return true;
                }
            }
            return false;
        }

        public void SelectHand(bool RightSide)
        {
            if(RightSide)
                ActiveHand = HandSide.Right;
            else
            {
                ActiveHand = HandSide.Left;
            }
        }
    }
}