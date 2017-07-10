using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public class ReagentManager : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {
            
        }

        static void TransferReagents(ReagentContainer source, ReagentContainer target, float amount)
        {
            source.NormalizeReagents();
            target.NormalizeReagents();

            if (target.AvailableVolume < amount)
                amount = target.AvailableVolume;

            foreach (var reagent in source.ReagentList)
            {
                float coef = reagent.Amount / source.Amount;
                float toTransfer = amount * coef;
                reagent.Amount -= toTransfer;
                Reagent copy = new Reagent(reagent.Name, toTransfer);
                target.AddReagent(copy);
            }

            source.NormalizeReagents();
            target.NormalizeReagents();
        }

        public static float AmountOfReagents(IEnumerable<Reagent> inputReagents)
        {
            float amount = 0;
            foreach (var reagent in inputReagents)
            {
                amount += reagent.Amount;
            }
            return amount;
        }
    }
}