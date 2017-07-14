﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    [Serializable]
    public class Damage
    {
        public float Brute;
        public float Burn;

        public Damage(float brute, float burn)
        {
            Brute = brute;
            Burn = burn;
        }

        public Damage()
        {
            Brute = 0;
            Burn = 0;
        }

        public static Damage operator -(Damage target, Damage source)
        {
            return new Damage(target.Brute - source.Brute, target.Burn - source.Burn);
        }
    }

    [Serializable]
    public class LifeSystem
    {
        public float BrainHealth = 100f;
        public float ToxinDamage = 0;
        public Damage HeadDamage = new Damage();
        public Damage LeftHanDamage = new Damage();
        public Damage RightHandDamage = new Damage();
        public Damage LeftLegDamage = new Damage();
        public Damage RightLegDamage = new Damage();
        public Damage ChestDamage = new Damage();
        public Damage GroinDamage = new Damage();
    }

    [Serializable]
    public enum State
    {
        Standing, Lying
    }

    [Serializable]
    public class Equipment
    {
        public enum EquipmentSlot { LeftHand, RightHand, Backpack}

        public const int AmountOfSlots = 3;

        public GameObject[] Inventory = new GameObject[AmountOfSlots];

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
