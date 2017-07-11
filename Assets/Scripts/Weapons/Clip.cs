using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    public class Clip : Storage
    {
        public WeaponModels WeaponModel;

        public bool RemoveBullet()
        {
            if (StoredList.Count > 0)
            {
                StoredList.RemoveAt(StoredList.Count - 1);
                return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            if (StoredList.Count > 0)
                return false;
            return true;
        }

    }
}