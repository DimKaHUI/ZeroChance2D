﻿using System.Collections;
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
                //uiManager.PlayerHuman.Equipment[Slot] = item;
                uiManager.PlayerCtrl.CmdSetupEquipment((int)Slot, item);
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
            //uiManager.PlayerHuman.Equipment[Slot] = null;
            uiManager.PlayerCtrl.CmdSetupEquipment((int)Slot, null);
        }

        // Use this for initialization
        void Start()
        {
            uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}