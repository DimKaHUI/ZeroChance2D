using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{

    public class PlayerEquipmentSync : NetworkBehaviour
    {
        [SyncVar] public Equipment SyncEquipment;

        void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                UpdateEquipment();
            }
            else
            {
                TransmitEquipment();
            }
        }


        void UpdateEquipment()
        {
            gameObject.GetComponent<Human>().Equipment.LeftHandItem = SyncEquipment.LeftHandItem;
            gameObject.GetComponent<Human>().Equipment.RightHandItem = SyncEquipment.RightHandItem;
        }

        [Client]
        void TransmitEquipment()
        {
            CmdSendEquipmentToServer(gameObject.GetComponent<Human>().Equipment);
        }

        [Command]
        void CmdSendEquipmentToServer(Equipment equipment)
        {
            SyncEquipment.LeftHandItem = equipment.LeftHandItem;
            if(SyncEquipment.LeftHandItem != null)
                SyncEquipment.LeftHandItem.GetComponent<Item>().Visible = false;
            SyncEquipment.RightHandItem = equipment.RightHandItem;
            if (SyncEquipment.RightHandItem != null)
                SyncEquipment.RightHandItem.GetComponent<Item>().Visible = false;
        }

    }
}


/*
 * string debugMsg = "Server data: Left: ";
            if (SyncEquipment.LeftHandItem == null)
                debugMsg += "none ";
            else
                debugMsg += SyncEquipment.LeftHandItem.GetComponent<Item>().ItemName;

            debugMsg += " Right: ";
            if (SyncEquipment.RightHandItem == null)
                debugMsg += "none ";
            else
                debugMsg += SyncEquipment.RightHandItem.GetComponent<Item>().ItemName;

            Debug.Log(debugMsg);
*/