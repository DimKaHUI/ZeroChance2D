using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{

    public class PlayerEquipmentSync : NetworkBehaviour
    {
        [SyncVar] public Equipment SyncEquipment = new Equipment(3);

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
            gameObject.GetComponent<Human>().Equipment = SyncEquipment;
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