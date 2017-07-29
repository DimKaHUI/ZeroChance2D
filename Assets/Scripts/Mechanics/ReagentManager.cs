using System;
using UnityEngine.Networking;

namespace ZeroChance2D.Assets.Scripts.Mechanics
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