using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.Items
{

    [RequireComponent(typeof(Storage))]
    public class Backpack : Item
    {
        private bool showsUi = false;
        private Storage storage;

        // Use this for initialization
        void Start()
        {
            BackHoldable = true;
            storage = gameObject.GetComponent<Storage>();
        }

        // Update is called once per frame
        void Update()
        {
            Visualization();
        }

        public override void Use()
        {
            storage.ShowGui(User);
        }
    }
}