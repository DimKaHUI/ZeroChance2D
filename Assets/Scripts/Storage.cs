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

        public virtual bool AddItem(Item item)
        {
            if (FreeSpace >= MaxSlots)
                return false;
            StoredList.Add(item);
            return true;
        }

        public virtual bool RemoveItem(Item item)
        {
            return StoredList.Remove(item);
        }

        public static TransferResult TransferItem(Storage source, Storage target, Item item)
        {
            if (!source.StoredList.Contains(item))
                return TransferResult.SourceHasNoItem;
            if(target.FreeSpace > item.SlotSize)
                return TransferResult.NoFreeSpace;
            source.StoredList.Remove(item);
            target.StoredList.Add(item);
            return TransferResult.Success;
        }
        public enum TransferResult { Success, SourceHasNoItem, NoFreeSpace}
    }
}