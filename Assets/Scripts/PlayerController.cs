﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ZeroChance2D
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

        public GameObject ManipulatedItem;

        private Rigidbody2D rig;
        private GameObject cameraObject;
        private Vector3 prevOffset = Vector3.zero;
        private Human playerHuman;
        private UIManger Ui;
        

        // Use this for initialization
        void Start()
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.z);

            if (isLocalPlayer)
            {
                Ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManger>();
                if(Ui == null)
                    Debug.LogWarning("No UIManager found!");

                Ui.LocalPlayerObject = gameObject;

                cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
                if (cameraObject == null)
                    Debug.LogError("Camera not found!");
                prevOffset = Vector3.zero;
                playerHuman = gameObject.GetComponent<Human>();
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
                Debug.Log("Mode changed");
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
                rig.velocity = rig.gameObject.transform.up * playerHuman.WalkSpeed * forward;

                Ray ray = cameraObject.GetComponent<Camera>()
                    .ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                var point = ray.GetPoint(CameraRelativePosition.z);

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

            if(Ui != null)
                ManipulatedItem = Ui.UnderCursor("Items");

            if (Input.GetMouseButtonDown(0) && ManipulatedItem != null)
            {
                PickItem(ManipulatedItem);
            }
            if (Input.GetMouseButtonDown(0) && ManipulatedItem == null)
            {
                if (ActiveHand == HandSide.Left && playerHuman.Equipment.LeftHandItem != null)
                    PutItem(ActiveHand);
                if (ActiveHand == HandSide.Right && playerHuman.Equipment.RightHandItem != null)
                    PutItem(ActiveHand);
            }
        }

        void PickItem(GameObject item)
        {
            switch (ActiveHand)
            {
                case HandSide.Left:
                    if (gameObject.GetComponent<Human>().Equipment.LeftHandItem == null)
                    {
                        item.GetComponent<Item>().Visible = false;
                        gameObject.GetComponent<Human>().Equipment.LeftHandItem = item;
                    }
                    break;
                case HandSide.Right:
                    if (gameObject.GetComponent<Human>().Equipment.RightHandItem == null)
                    {
                        item.GetComponent<Item>().Visible = false;
                        gameObject.GetComponent<Human>().Equipment.RightHandItem = item;
                    }
                    break;
            }
        }

        void PutItem(HandSide side)
        {
            switch (side)
            {
                case HandSide.Left:
                    CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment.LeftHandItem, Ui.UnderCursorPoint);
                    gameObject.GetComponent<Human>().Equipment.LeftHandItem = null;
                    break;
                case HandSide.Right:
                    CmdSetItemLocation(gameObject.GetComponent<Human>().Equipment.RightHandItem, Ui.UnderCursorPoint);
                    gameObject.GetComponent<Human>().Equipment.RightHandItem = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }
        }

        [Command]
        void CmdSetItemLocation(GameObject item, Vector3 location)
        {
            item.transform.position = location;
            item.GetComponent<Item>().Visible = true;
        }

        

    }
}

