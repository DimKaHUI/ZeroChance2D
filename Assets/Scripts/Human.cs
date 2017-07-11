using System;
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

    public class Equipment
    {
        public Item LeftHandItem;
        public Item RightHandItem;

        public Equipment()
        {
            LeftHandItem = null;
            RightHandItem = null;
        }
    }

    [Serializable]
    public class Human : NetworkBehaviour
    {

        public float WalkSpeed;
        public float RunSpeed;
        public LifeSystem LifeParams;
        public float SleepThresold = 100f;
        public float Nutrition = 100f;
        public State State;
        public Equipment Equipment = new Equipment();

    }
}
