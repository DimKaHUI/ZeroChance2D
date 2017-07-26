using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{

    public class CExpButton : MonoBehaviour
    {
        public ZeroChance2D.CrateExplorer Explorer;
        public GameObject ItemObj;
        public void Click()
        {
            Explorer.ItemCallback(gameObject, ItemObj);
        }
    }
}
