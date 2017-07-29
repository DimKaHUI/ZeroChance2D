using UnityEngine;
using UnityEngine.Networking;
using ZeroChance2D.Assets.Scripts.Items;

namespace ZeroChance2D.Assets.Scripts.Mechanics
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

        [ServerCallback]
        void UpdateItemState()
        {
            if (isServer)
                SyncVisible = item.Visible;
        }

        [ClientCallback]
        void ReceiveItemStatey()
        {
            if (!isServer)
                item.Visible = SyncVisible;
        }
    }
}
