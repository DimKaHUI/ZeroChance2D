using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    [RequireComponent(typeof(Item))]
    public class ItemSync : NetworkBehaviour
    {
        [SyncVar]
        public bool SyncVisible = true;

        private Item item;

        void Start()
        {
            item = gameObject.GetComponent<Item>();
        }

        void Update()
        {
            UpdateItemState();
            ReceiveItemStatey();
        }

        [Server]
        void UpdateItemState()
        {
            if (isServer)
                SyncVisible = item.Visible;
        }

        [Client]
        void ReceiveItemStatey()
        {
            if (!isServer)
                item.Visible = SyncVisible;
        }
    }
}
