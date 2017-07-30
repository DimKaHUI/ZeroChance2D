using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.UI
{

    public class EquipmentUiSlot : MonoBehaviour, IDropTarget, IDragTarget
    {
        public Equipment.EquipmentSlot Slot;

        private UIManager uiManager;

        public virtual bool ItemDragEnd(GameObject item)
        {
            if (uiManager == null)
                return false;
            if (uiManager.PlayerHuman.Equipment[Slot] == null)
            {
                uiManager.PlayerCtrl.CmdSetupEquipment((int)Slot, item);
                uiManager.PlayerCtrl.CmdSetupItem(item, uiManager.PlayerHuman.gameObject, (HandSide)Slot, false);
                return true;
            }
            return false;
        }

        public virtual GameObject DragGameObject
        {
            get { return uiManager.PlayerHuman.Equipment[Slot]; }
        }

        public virtual void OnSuccessfullDrag()
        {
            uiManager.PlayerCtrl.CmdSetupEquipment((int)Slot, null);
        }

        void Start()
        {
            uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        }

        void Update()
        {

        }
    }
}