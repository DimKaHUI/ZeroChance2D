using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    public class ColaCan : Item
    {

        private float initWeight;
        private ReagentContainer colaCan;

        // Use this for initialization
        void Start()
        {
            initWeight = Weight;
            colaCan = gameObject.GetComponent<ReagentContainer>();
        }

        // Update is called once per frame
        void Update()
        {
            Weight = initWeight + colaCan.Amount / 1000f;
        }
    }
}