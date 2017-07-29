using UnityEngine.Networking;
using ZeroChance2D.Assets.Scripts.Items;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{

    public class PlayerEquipmentSync : NetworkBehaviour
    {
        [SyncVar] public Equipment SyncEquipment = new Equipment();

        void FixedUpdate()
        {
            UpdateEquipment();
            DownloadEquipment();
        }

        [ServerCallback]
        void UpdateEquipment()
        {
            if (!isServer)
                return;
            if (!gameObject.GetComponent<Human>().Equipment.IsEqual(SyncEquipment))
            {
                SyncEquipment = (Equipment)gameObject.GetComponent<Human>().Equipment.Clone();
            }
        }

        [ClientCallback]
        void DownloadEquipment()
        {
            if (isServer)
                return;
            if (!gameObject.GetComponent<Human>().Equipment.IsEqual(SyncEquipment))
            {
                gameObject.GetComponent<Human>().Equipment = (Equipment) SyncEquipment.Clone();
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