using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ZeroChance2D.Assets.Scripts.Items;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{
    [Serializable]
    public class Inventory: ICloneable
    {
        public GameObject[] StoredList = new GameObject[0];

        public Inventory(int size)
        {
            StoredList = new GameObject[size];
        }

        public Inventory()
        {
            
        }

        public object Clone()
        {
            var inv = new Inventory();
            inv.StoredList = new GameObject[StoredList.Length];
            for (int i = 0; i < StoredList.Length; i++)
            {
                inv.StoredList[i] = StoredList[i];
            }
            return inv;
        }

        public int IndexOf(GameObject itemGameObject)
        {
            for (int i = 0; i < StoredList.Length; i++)
            {
                if (StoredList[i] == itemGameObject)
                    return i;
            }
            return -1;
        }

        /*public  bool IsEqual(Inventory equipment)
        {
            
            if (StoredList == null && equipment.StoredList == null)
                return true;

            if (StoredList == null && equipment.StoredList != null)
                return false;
            
            if (StoredList != null && equipment.StoredList == null)
                return false;

            if (equipment.StoredList != null && StoredList != null)
            {
                if (equipment.StoredList.Length != StoredList.Length)
                    return false;

                for (int i = 0; i < StoredList.Length; i++)
                {
                    if (StoredList[i] != equipment.StoredList[i])
                        return false;
                }
            }
            return true;
        }
        
        public static bool operator ==(Inventory a, Inventory b)
        {
            if (a.IsEqual(b))
                return true;
            return false;
        }
        public static bool operator !=(Inventory a, Inventory b)
        {
            return !(a == b);
        }*/
    }
    public class Storage : NetworkBehaviour
    {
        public GameObject GuiPrefab;
        public int MaxSlots;
        public Inventory Inventory = new Inventory(0);
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

        [ServerCallback]
        public virtual TransferResult AddItem(GameObject itemObj)
        {
            if(itemObj == gameObject)
                return TransferResult.SelfStoring;
            if(itemObj.GetComponent<Item>() == null)
                return TransferResult.NotAnItem;
            if (itemObj.GetComponent<Item>().ItemName != MandatoryItemName && MandatoryItemName != "")
                return TransferResult.UnsuitableItem;
            if (itemObj.GetComponent<Item>().SlotSize > MaximumItemSize && MaximumItemSize > 0)
                return TransferResult.TooLargeItem;
            if (FreeSpace <= 0)
                return TransferResult.NoFreeSpace;
            if(Inventory.IndexOf(itemObj) != -1)
                return TransferResult.AlreadyContains;
            Array.Resize(ref Inventory.StoredList, Inventory.StoredList.Length + 1);
            Inventory.StoredList[Inventory.StoredList.Length - 1] = itemObj;
            itemObj.GetComponent<Item>().User = gameObject;
            return TransferResult.Success;
        }

        [ServerCallback]
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
        public enum TransferResult { Success, SourceHasNoItem, NoFreeSpace, UnsuitableItem, TooLargeItem, NotAnItem, AlreadyContains, SelfStoring }

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