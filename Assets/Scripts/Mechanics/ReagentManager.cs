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
        private float amount;
        public readonly string Name;

        public float Amount
        {
            get { return amount; }
            set
            {
                if (value >= 0)
                    amount = value;
                else
                    amount = 0;
            }
        }

        public Reagent(string name, float amount)
        {
            Name = name;
            this.amount = amount;
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

        static void TransferReagents(IReagentContainer source, IReagentContainer target, float amount)
        {
            source.NormalizeReagents();
            target.NormalizeReagents();

            if (target.SpaceLeft < amount)
                amount = target.SpaceLeft;

            foreach (var reagent in source.Reagents)
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