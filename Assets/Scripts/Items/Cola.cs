using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{



    public class Cola : MonoBehaviour, IItem, IReagentContainer
    {
        private int slotSize = 5;
        private List<IReagent> reagents = new List<IReagent>();
        private float volume = 0.33f;
        



        /// <summary>
        /// Количество занимаемых слотов инвентаря
        /// </summary>
        public int SlotSize
        {
            get { return slotSize; }
        }



        /// <summary>
        /// Вес в килограммах
        /// </summary>
        public float Weight
        {
            get { return 0.01f; }
        }




        public float Volume { get { return volume; } }



        public float Amount
        {
            get {return summVol(reagents); }
        }


        public void AddReagents(ref List<IReagent> inputReagents)
        {
            
        }

        public float TransferReagents(ref IReagentContainer target, float amount)
        {
            return 0f;
        }

        public float FlushReagents()
        {
            float amount = Amount;
            reagents = new List<IReagent>();
            return amount;
        }


        private float summVol(IEnumerable<IReagent> inputReagents)
        {
            float amount = 0;
            foreach (var r in inputReagents)
                amount += r.Amount;
            return amount;
        }


        
        public void AddReagent(ref IReagent reagent, float amount)
        {
            float availableAmount = Volume - Amount;
            for (int i = 0; i < reagents.Count; i++)
            {
                if (reagents[i].ReagentName == reagent.ReagentName)
                {
                    if (reagent.Amount > availableAmount)
                    {
                        reagent.Amount -= availableAmount;
                        reagents[i].Amount += availableAmount;
                    }
                    else
                    {
                        reagents[i].Amount += reagent.Amount;
                        reagent.Amount = 0;
                    }
                    return;
                }
            }

            reagents.Add(reagent);
            if (reagent.Amount > availableAmount)
            {
                reagents[reagents.Count - 1].Amount = availableAmount;
                reagent.Amount -= availableAmount;
            }
            else
            {
                reagent.Amount = 0;
            }
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