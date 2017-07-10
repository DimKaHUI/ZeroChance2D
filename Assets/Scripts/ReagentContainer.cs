using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
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

        public virtual float Amount { get { return ReagentManager.AmountOfReagents(ReagentList); } }
        public virtual float AvailableVolume { get { return Volume - Amount; } }

        public virtual float FlushReagents()
        {
            float amount = Amount;
            //ReagentList = new Reagent[0];
            ReagentList = new List<Reagent>();
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