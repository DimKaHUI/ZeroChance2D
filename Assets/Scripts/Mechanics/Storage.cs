﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ZeroChance2D
{
    [Serializable]
    public struct Inventory
    {
        public GameObject[] StoredList;
    }
    public class Storage : NetworkBehaviour
    {
        public GameObject GuiPrefab;
        public int MaxSlots;
        [SyncVar]
        public Inventory Inventory;
        public string MandatoryItemName;
        public int MaximumItemSize;

        public int FreeSpace
        {
            get { return MaxSlots - UsedSlots; }
        }

        public int UsedSlots
        {
            get
            {
                int slots = 0;
                foreach (var itemObj in Inventory.StoredList)
                {
                    slots += itemObj.GetComponent<Item>().SlotSize;
                }
                return slots;
            }
        }

        public virtual TransferResult AddItem(GameObject itemObj)
        {
            if (itemObj.GetComponent<Item>().ItemName != MandatoryItemName && MandatoryItemName != "")
                return TransferResult.UnsuitableItem;
            if (itemObj.GetComponent<Item>().SlotSize > MaximumItemSize && MaximumItemSize > 0)
                return TransferResult.TooLargeItem;
            if (FreeSpace <= 0)
                return TransferResult.NoFreeSpace;
            Array.Resize(ref Inventory.StoredList, Inventory.StoredList.Length + 1);
            Inventory.StoredList[Inventory.StoredList.Length - 1] = itemObj;
            return TransferResult.Success;
        }

        public virtual bool RemoveItem(GameObject item)
        {
            int len = Inventory.StoredList.Length;
            Inventory.StoredList = ExcludeFromArray(Inventory.StoredList, item);
            return len == Inventory.StoredList.Length;
        }

        public static T[] ExcludeFromArray<T>(T[] array, T element) where T : class
        {

            int count = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] == element)
                {
                    array[i] = null;
                    count++;
                    for (int j = i; j < array.Length - 1; j++)
                    {
                        array[j] = array[j + 1];
                    }
                }

            Array.Resize(ref array, array.Length - count);
            return array;
        }

        public static TransferResult TransferItem(Storage source, Storage target, GameObject itemObj)
        {
            if (itemObj.GetComponent<Item>().ItemName != target.MandatoryItemName && target.MandatoryItemName != "")
                return TransferResult.UnsuitableItem;
            if (itemObj.GetComponent<Item>().SlotSize > target.MaximumItemSize)
                return TransferResult.TooLargeItem;
            //if (!source.StoredList.Contains(itemObj))
                //return TransferResult.SourceHasNoItem;
            if (target.FreeSpace > itemObj.GetComponent<Item>().SlotSize)
                return TransferResult.NoFreeSpace;
            source.RemoveItem(itemObj);
            target.AddItem(itemObj);
            return TransferResult.Success;
        }
        public enum TransferResult { Success, SourceHasNoItem, NoFreeSpace, UnsuitableItem, TooLargeItem}

        public virtual void ShowGui(GameObject user)
        {
            var canvas = GameObject.Find("Canvas");
            canvas.transform.Find("UiMaskPanel").GetComponent<Image>().raycastTarget = true;
            var gui = Instantiate(GuiPrefab);
            gui.transform.SetParent(canvas.transform);
            gui.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            gui.GetComponent<CrateExplorer>().AttachedStorage = this;
            gui.GetComponent<CrateExplorer>().User = user;
        }
    }
}