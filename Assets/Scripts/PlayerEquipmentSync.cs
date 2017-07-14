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
            gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.LeftHand] = SyncEquipment[Equipment.EquipmentSlot.LeftHand];
            gameObject.GetComponent<Human>().Equipment[Equipment.EquipmentSlot.RightHand] = SyncEquipment[Equipment.EquipmentSlot.RightHand];
        }

        [Client]
        void TransmitEquipment()
        {
            if (!SyncEquipment.IsEqual(gameObject.GetComponent<Human>().Equipment))
                CmdSendEquipmentToServer(gameObject.GetComponent<Human>().Equipment);
        }

        [Command]
        void CmdSendEquipmentToServer(Equipment equipment)
        {
            /*SyncEquipment[Equipment.EquipmentSlot.LeftHand] = equipment[Equipment.EquipmentSlot.LeftHand];
            if (SyncEquipment[Equipment.EquipmentSlot.LeftHand] != null)
                SyncEquipment[Equipment.EquipmentSlot.LeftHand].GetComponent<Item>().Visible = false;

            SyncEquipment[Equipment.EquipmentSlot.RightHand] = equipment[Equipment.EquipmentSlot.RightHand];
            if (SyncEquipment[Equipment.EquipmentSlot.RightHand] != null)
                SyncEquipment[Equipment.EquipmentSlot.RightHand].GetComponent<Item>().Visible = false;*/

            Debug.Log("CmdSendEquipmentToServer command received");

            for (Equipment.EquipmentSlot i = 0; i < (Equipment.EquipmentSlot)Equipment.AmountOfSlots; i++)
            {
                SyncEquipment[i] = equipment[i];
                if (SyncEquipment[i] != null)
                {
                    SyncEquipment[i].GetComponent<Item>().Visible = false;
                }
            }
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