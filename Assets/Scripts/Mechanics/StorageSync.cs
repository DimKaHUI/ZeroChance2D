using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{
    [RequireComponent(typeof(Storage))]
    public class StorageSync : NetworkBehaviour
    {
        [SyncVar]
        public Inventory SyncInventory;

        private Inventory prevInv;

        private Storage storage;

        void Start()
        {
            storage = gameObject.GetComponent<Storage>();
        }

        void Update()
        {
            UpdateInventory();
            ReceiveInventory();
            //SendInvChange();
        }

        [Server]
        void UpdateInventory()
        {
            if (isServer)
                SyncInventory = (Inventory)storage.Inventory.Clone();
        }

        [Client]
        void ReceiveInventory()
        {
            if (!isServer)
                storage.Inventory = SyncInventory;
        }

        /*[Client]
        public void SendInvChange()
        {
            if (SyncInventory != prevInv)
            {
                var cExp = GameObject.FindGameObjectWithTag("CrateExplorer");
                if (cExp != null)
                    cExp.GetComponent<CrateExplorer>().SendMessage("OnInvChange");
            }
            prevInv = SyncInventory;
        }*/
    }
}
