using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.UI
{
    public interface IStorageUi: IDropTarget
    {
        Storage AttachedStorage { get; set; }
        GameObject User { get; set; }

        void DropEndCallback(GameObject button, GameObject itemObj);

        void ItemCallback(GameObject button, GameObject itemObj);
    }
}
