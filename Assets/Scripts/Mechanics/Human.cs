using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
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
        Standing, Lying
    }

    [Serializable]
    public struct Equipment
    {
        public enum EquipmentSlot { LeftHand, RightHand, Backpack}

        public const int AmountOfSlots = 3;

        public GameObject[] Inventory;

        public GameObject this[EquipmentSlot slot]
        {
            get { return Inventory[(int) slot]; }
            set { Inventory[(int) slot] = value; }
        }

        public GameObject this[int slot]
        {
            get { return Inventory[slot]; }
            set { Inventory[slot] = value; }
        }

        public bool IsEqual(Equipment equipment)
        {
            for (EquipmentSlot i = 0; i < (EquipmentSlot)AmountOfSlots; i++)
            {
                if(this[i] != equipment[i])
                    return false;
            }

            return true;
        }

        public Equipment(int amount)
        {
            Inventory = new GameObject[amount];
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
        public Equipment Equipment = new Equipment(Equipment.AmountOfSlots);
    }
}
