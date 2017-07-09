using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{

    public interface IItem
    {
        float Weight { get; }
        int SlotSize { get; }
    }
}