using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.Weapons
{
    public class AmmoClip : Storage
    {
        public WeaponModels WeaponModel;

        public bool RemoveBullet()
        {
            if (Inventory.StoredList.Length > 0)
            {
                RemoveItem(Inventory.StoredList[Inventory.StoredList.Length - 1]);
                return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            if (Inventory.StoredList.Length > 0)
                return false;
            return true;
        }

    }
}