using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{
    [Serializable]
    public struct Damage
    {
        public float Brute;
        public float Burn;

        public Damage(float brute, float burn)
        {
            Brute = brute;
            Burn = burn;
        }

        public static Damage operator -(Damage target, Damage source)
        {
            return new Damage(target.Brute - source.Brute, target.Burn - source.Burn);
        }
    }

    [Serializable]
    public struct LifeSystem
    {
        public float BrainHealth;
        public float ToxinDamage;
        public Damage HeadDamage;
        public Damage LeftHandDamage;
        public Damage RightHandDamage;
        public Damage LeftLegDamage;
        public Damage RightLegDamage;
        public Damage ChestDamage;
        public Damage GroinDamage;

        public LifeSystem(float brainDamage)
        {
            BrainHealth = brainDamage;
            ToxinDamage = 0f;
            HeadDamage = new Damage(0, 0);
            LeftHandDamage = new Damage(0, 0);
            RightHandDamage = new Damage(0, 0);
            LeftLegDamage = new Damage(0, 0);
            RightLegDamage = new Damage(0, 0);
            ChestDamage = new Damage(0, 0);
            GroinDamage = new Damage(0, 0);
        }
    }

    [Serializable]
    public enum State
    {
        Standing,
        Lying
    }

    [Serializable]
    public class Equipment : ICloneable
    {
        public enum EquipmentSlot
        {
            LeftHand,
            RightHand,
            Backpack
        }

        public const int AmountOfSlots = 3;

        [SerializeField] public GameObject[] inventory = new GameObject[AmountOfSlots];

        public GameObject this[EquipmentSlot slot]
        {
            get { return inventory[(int) slot]; }
            set { inventory[(int) slot] = value; }
        }

        public GameObject this[int slot]
        {
            get { return inventory[slot]; }
            set { inventory[slot] = value; }
        }

        public bool IsEqual(Equipment equipment)
        {
            for (EquipmentSlot i = 0; i < (EquipmentSlot) AmountOfSlots; i++)
            {
                if (this[i] != equipment[i])
                    return false;
            }

            return true;
        }

        public int IndexOf(GameObject element)
        {
            for (int i = 0; i < AmountOfSlots; i++)
            {
                if (inventory[i] == element)
                    return i;
            }

            return -1;
        }


        public Equipment()
        {
            inventory = new GameObject[AmountOfSlots];
        }

        public object Clone()
        {
            Equipment clone = new Equipment();
            for (int i = 0; i < AmountOfSlots; i++)
            {
                clone[i] = inventory[i];
            }
            return clone;
        }
    }

    [Serializable]
    public class Human : NetworkBehaviour
    {
        public float WalkSpeed;
        public float RunSpeed;
        public LifeSystem LifeParams = new LifeSystem();
        public float SleepThresold = 100f;
        public float Nutrition = 100f;
        public State State;
        public float RotationSpeed = 60f;
        public Equipment Equipment = new Equipment();
    }
}