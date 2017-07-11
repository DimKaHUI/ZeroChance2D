using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{

    public class Storage : NetworkBehaviour
    {
        public int MaxSlots;
        public List<Item> StoredList;
        public string MandatoryItemName;
        public int MaximumItemSize;

        public int FreeSpace
        {
            get
            {
                int slots = 0;
                foreach (var item in StoredList)
                {
                    slots += item.SlotSize;
                }
                return slots;
            }
        }

        public virtual TransferResult AddItem(Item item)
        {
            if (item.ItemName != MandatoryItemName && MandatoryItemName != "")
                return TransferResult.UnsuitableItem;
            if (item.SlotSize > MaximumItemSize)
                return TransferResult.TooLargeItem;
            if (FreeSpace >= MaxSlots)
                return TransferResult.NoFreeSpace;
            StoredList.Add(item);
            return TransferResult.Success;
        }

        public virtual bool RemoveItem(Item item)
        {
            return StoredList.Remove(item);
        }

        public static TransferResult TransferItem(Storage source, Storage target, Item item)
        {
            if (item.ItemName != target.MandatoryItemName && target.MandatoryItemName != "")
                return TransferResult.UnsuitableItem;
            if(item.SlotSize > target.MaximumItemSize)
                return TransferResult.TooLargeItem;
            if (!source.StoredList.Contains(item))
                return TransferResult.SourceHasNoItem;
            if(target.FreeSpace > item.SlotSize)
                return TransferResult.NoFreeSpace;
            source.StoredList.Remove(item);
            target.StoredList.Add(item);
            return TransferResult.Success;
        }
        public enum TransferResult { Success, SourceHasNoItem, NoFreeSpace, UnsuitableItem, TooLargeItem}
    }
}