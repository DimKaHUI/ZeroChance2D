using System;
using UnityEngine;
using UnityEngine.Networking;
using ZeroChance2D.Assets.Scripts.Items;
using ZeroChance2D.Assets.Scripts.UI;
using ZeroChance2D.Assets.Scripts.Weapons;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{
    public enum HandSide
    {
        Left,
        Right
    }

    public enum ControllMode
    {
        Interaction,
        Shooting
    }

    public class PlayerController : NetworkBehaviour
    {
        public HandSide ActiveHand;
        public ControllMode CurrentMode;

        public Vector3 CameraRelativePosition = new Vector3(0, 0, -10);
        public float AimSensitivity = 1f;
        public float ItemPickRange = 3f;
        public float ShootingModeMovementThresold = 0.5f;

        public GameObject ManipulatedItem;

        private Rigidbody2D rig;
        private GameObject cameraObject;
        private Vector3 prevOffset = Vector3.zero;
        private Human playerHuman;
        private UIManager Ui;
        

        // Use this for initialization
        void Start()
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.z);
            playerHuman = gameObject.GetComponent<Human>();

            if (isLocalPlayer)
            {
                Ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
                if(Ui == null)
                    Debug.LogWarning("No UIManager found!");

                Ui.LocalPlayerObject = gameObject;

                cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
                if (cameraObject == null)
                    Debug.LogError("Camera not found!");
                prevOffset = Vector3.zero;
                rig = gameObject.GetComponent<Rigidbody2D>();
            }

        }

        void FixedUpdate()
        {

            if (!isLocalPlayer)
                return;

            #region Movement and camera controlling

            Vector3 pos = gameObject.transform.position;


            if (playerHuman == null)
            {
                Debug.LogWarning("PlayerController class not attached");
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
            }

            if (CurrentMode == ControllMode.Interaction)
            {
                float forward = Input.GetAxis("Vertical");
                float turn = Input.GetAxis("Horizontal");
                rig.velocity = rig.gameObject.transform.up * playerHuman.WalkSpeed * forward;
                rig.angularVelocity = -turn * playerHuman.RotationSpeed;
            }
            if (CurrentMode == ControllMode.Shooting)
            {
                float forward = Input.GetAxis("Vertical");
                float turn = Input.GetAxis("Horizontal");

                Ray ray = cameraObject.GetComponent<Camera>()
                    .ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                var point = ray.GetPoint(-CameraRelativePosition.z);

                if (Vector3.Distance(gameObject.transform.position, point) >= ShootingModeMovementThresold)
                {
                    rig.velocity = rig.gameObject.transform.up * playerHuman.WalkSpeed * forward;
                }
                else
                {
                    rig.velocity = Vector2.zero;
                }

                float angle = Mathf.Atan2(point.y - pos.y, point.x - pos.x);
                rig.gameObject.transform.rotation =
                    Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90);

            }

            if (CurrentMode == ControllMode.Interaction)
                cameraObject.transform.position = pos + CameraRelativePosition;
            if (CurrentMode == ControllMode.Shooting)
            {
                float move_x = Input.GetAxis("Mouse X");
                float move_y = Input.GetAxis("Mouse Y");
                prevOffset += new Vector3(move_x, move_y, 0) * AimSensitivity;
                cameraObject.transform.position = new Vector3(pos.x, pos.y, CameraRelativePosition.z) + prevOffset;

            }

            #endregion

            if (CurrentMode == ControllMode.Interaction)
            {
                #region Picking and puting items

                if (Ui != null && !Ui.IsCursorUponUi() && Ui.UnderCursor("Items") != null &&
                    Ui.UnderCursor("Items").layer != LayerMask.NameToLayer("Hidden"))
                {
                    ManipulatedItem = Ui.UnderCursor("Items");
                }
                else
                {
                    ManipulatedItem = null;
                }
                if (Input.GetMouseButtonDown(0) && ManipulatedItem != null && !Ui.IsCursorUponUi())
                {
                    PickItem(ManipulatedItem);
                }
                if (Input.GetMouseButtonDown(0) && ManipulatedItem == null && Ui.IsCursorUponUi() == false)
                {
                    if (ActiveHand == HandSide.Left && playerHuman.Equipment[Equipment.EquipmentSlot.LeftHand] != null)
                        PutItem(ActiveHand);
                    if (ActiveHand == HandSide.Right && playerHuman.Equipment[Equipment.EquipmentSlot.RightHand] !=
                        null)
                        PutItem(ActiveHand);
                }

                #endregion

                ShowMovablesGui();
            }

            #region Shooting

            // Getting a weapon instance
            if (CurrentMode == ControllMode.Shooting)
            {
                Weapon weapon = null;
                switch (ActiveHand)
                {
                    case HandSide.Left:
                        if (playerHuman.Equipment[0] == null)
                        {
                            // TODO Weapon-free behavior
                            break;
                        }
                        weapon = playerHuman.Equipment[0].GetComponent<Weapon>();
                        if (weapon == null)
                        {
                            // TODO Item is not a weapon issue
                            break;
                        }
                        weapon = playerHuman.Equipment[0].GetComponent<Weapon>();
                        break;
                    case HandSide.Right:
                        if (playerHuman.Equipment[1] == null)
                        {
                            // TODO Weapon-free behavior
                            break;
                        }
                        weapon = playerHuman.Equipment[1].GetComponent<Weapon>();
                        if (weapon == null)
                        {
                            // TODO Item is not a weapon issue
                            break;
                        }
                        weapon = playerHuman.Equipment[1].GetComponent<Weapon>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                // TODO Shooting
                if (weapon != null)
                {
                    if (weapon.ReadyToShoot && Input.GetAxis("Fire and usage") == 1f)
                        weapon.CmdUse(gameObject);
                }
                    
            }

            
            

            #endregion
        }


        [Command]
        public void CmdSetupEquipment(int slot, GameObject item)
        {
            gameObject.GetComponent<Human>().Equipment[slot] = item;
        }

        public void PickItem(GameObject item)
        {
            switch (ActiveHand)
            {
                case HandSide.Left:
                    if (gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.LeftHand] == null)
                    {
                        CmdSetupEquipment(0, item);
                        CmdSetupItem(item, gameObject, ActiveHand, false);
                    }
                    break;
                case HandSide.Right:
                    if (gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.RightHand] == null)
                    {
                        CmdSetupEquipment(1, item);
                        CmdSetupItem(item, gameObject, ActiveHand, false);
                    }
                    break;
            }
        }
        [Obsolete("Use PutItem(HandSide, Vector2) instead")]
        public void PutItem(HandSide side)
        {
            switch (side)
            {
                case HandSide.Left:
                    CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.LeftHand], Ui.PointToWorldLoc(Input.mousePosition));
                    CmdSetupItem(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.LeftHand], null, ActiveHand, true);
                    CmdSetupEquipment(0, null);
                    break;
                case HandSide.Right:
                    CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.RightHand], Ui.PointToWorldLoc(Input.mousePosition));
                    CmdSetupItem(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.RightHand], null, ActiveHand, true);
                    CmdSetupEquipment(1, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }
        }

        public void PutItem(HandSide side, Vector2 position)
        {
            switch (side)
            {
                case HandSide.Left:
                    CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.LeftHand], position);
                    CmdSetupItem(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.LeftHand], null, ActiveHand, true);
                    CmdSetupEquipment(0, null);
                    break;
                case HandSide.Right:
                    CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.RightHand], position);
                    CmdSetupItem(gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.RightHand], null, ActiveHand, true);
                    CmdSetupEquipment(1, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }
        }

        public void PutItem(GameObject item, Vector2 position)
        {
            int index = playerHuman.Equipment.IndexOf(item);
            if (index != -1)
            {
                CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment[index], position);
                CmdSetupItem(gameObject.GetComponent<Human>().Equipment[index], null, ActiveHand, true);
                CmdSetupEquipment(index, null);
            }
        }

        //delegate void ShowStorageGui(GameObject user);
        void ShowMovablesGui()
        {
            if (Ui.UnderCursor("Items") != null || Ui.IsCursorUponUi())
            {
                return;
            }
            if (playerHuman.Equipment[(int) ActiveHand] != null)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                GameObject movable = Ui.UnderCursor("Movables");
                if (movable != null)
                {
                    //Storage storage = movable.GetComponent<Storage>();
                    //ShowStorageGui showGui = storage.ShowGui;
                    movable.GetComponent<Storage>().ShowGui(gameObject);
                }
            }

        }

        [Command]
        public void CmdRemoveFromStorage(GameObject storage, GameObject item)
        {
            item.GetComponent<Item>().User = gameObject;
            storage.GetComponent<Storage>().RemoveItem(item);
        }

        [Command]
        public void CmdAddToStorage(GameObject storage, GameObject item)
        {
            int index = playerHuman.Equipment.IndexOf(item);
            if (index != -1)
            {
                storage.GetComponent<Storage>().AddItem(item);
                item.GetComponent<Item>().User = null;
            }
            else {Debug.LogWarning("Player does not have this Item instance");}

        }

        [Command]
        public void CmdPutIntoStorage(GameObject storage, int slot)
        {
            if (storage.GetComponent<Storage>().AddItem(playerHuman.Equipment[slot]) == Storage.TransferResult.Success)
            {
                playerHuman.Equipment[slot].GetComponent<Item>().User = storage;
                playerHuman.Equipment[slot].GetComponent<Item>().Visible = false;
                playerHuman.Equipment[slot] = null;
            }
        }
        

        [Command]
        public void CmdSetupItem(GameObject item, GameObject owner, HandSide side, bool visible)
        {
            item.GetComponent<Item>().Visible = visible;
            item.GetComponent<Item>().User = owner;
            item.GetComponent<Item>().HandSide = side;
        }

        [Command]
        public void CmdHandledItemClick(GameObject player, bool rightSide)
        {
            var human = player.GetComponent<Human>();
            if (rightSide)
            {
                if (human.Equipment[1] != null)
                    human.Equipment[1].GetComponent<Item>().Use();

            }
            else
            {
                if (human.Equipment[0] != null)
                    human.Equipment[0].GetComponent<Item>().Use();
            }
        }

        [Command]
        public void CmdSetItemLocation(GameObject item, Vector3 location)
        {
            item.transform.position = location;
            item.GetComponent<Item>().Visible = true;
            item.GetComponent<Item>().User = null;
        }

        

    }
}

