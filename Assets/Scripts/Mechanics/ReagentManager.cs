using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ZeroChance2D;

namespace ZeroChance2D
{
    [Serializable]
    public class Reagent
    {
        public float Amount;
        public string Name;

        public Reagent(string name, float amount)
        {
            Name = name;
            this.Amount = amount;
        }
    }

    public class ReagentManager : NetworkBehaviour
    {

        void Start()
        {

        }

        void Update()
        {
            
        }
    }
}