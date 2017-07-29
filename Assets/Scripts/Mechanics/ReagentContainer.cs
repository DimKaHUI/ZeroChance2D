using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{

    public class ReagentContainer : MonoBehaviour
    {
        public float Volume;

        //public Reagent[] ReagentList = new Reagent[0];
        public List<Reagent> ReagentList = new List<Reagent>();

        public virtual float AddReagent(Reagent reagent)
        {
            throw new NotImplementedException();
        }

        public virtual void AddReagents(Reagent[] inputReagents)
        {
            throw new NotImplementedException();
        }

        public void NormalizeReagents()
        {
            
        }

        public virtual float Amount { get { return AmountOfReagents(ReagentList); } }
        public virtual float AvailableVolume { get { return Volume - Amount; } }

        public virtual float FlushReagents()
        {
            float amount = Amount;
            //ReagentList = new Reagent[0];
            ReagentList = new List<Reagent>();
            return amount;
        }

        private static void TransferReagents(ReagentContainer source, ReagentContainer target, float amount)
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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}