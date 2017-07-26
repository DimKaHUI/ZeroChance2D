using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    [RequireComponent(typeof(Storage))]
    public class StorageSync : NetworkBehaviour
    {
        [SyncVar]
        public Inventory SyncInventory;

        private Storage storage;

        void Start()
        {
            storage = gameObject.GetComponent<Storage>();
        }

        void Update()
        {
            UpdateInventory();
            ReceiveInventory();
        }

        [Server]
        void UpdateInventory()
        {
            if (isServer)
                SyncInventory = storage.Inventory;
        }

        [Client]
        void ReceiveInventory()
        {
            if (!isServer)
                storage.Inventory = SyncInventory;
        }
    }
}
