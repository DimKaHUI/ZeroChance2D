using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{

    public interface ICrate
    {

        int SlotVolume { get; }
        List<IItem> StoredItems { get; set; }
        bool AddItem(IItem item);
        bool RemoveItem(IItem item);
    }

}
