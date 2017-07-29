using UnityEngine;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.Items
{
    [RequireComponent(typeof(Collider2D))]
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
            Visualization();
            Weight = initWeight + colaCan.Amount / 1000f;
        }
    }
}