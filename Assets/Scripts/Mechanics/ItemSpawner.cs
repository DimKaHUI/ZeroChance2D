using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace ZeroChance2D
{
    [Serializable]
    public class ItemSpawnSetup
    {
        public GameObject Item;
        [Range(0f, 1f)]
        public float Chance = 1f;
        //public int Count = 1;
    }

    public class ItemSpawner : MonoBehaviour
    {
        public ItemSpawnSetup[] SpawnableItems;
        [Range(0f, 1f)]
        public float Chance = 1f;
        public int Count = 1;
        [Tooltip("If count of items is more than one, each next item will be spawned with an offset from previous one")]
        public Vector3 PerItemOffset = new Vector3(0, 0, 0);
        [Tooltip("If not null, item will be added to the storage after spawning.")]
        public Storage Storage;
        public bool DestroyUponCall = true;

        private bool calculateChance()
        {
            if (Random.value <= Chance)
                return true;
            return false;
        }

        public void Spawn()
        {
            if (calculateChance())
            {

                for (int i = 0; i < Count; i++)
                {
                    GameObject item = Instantiate(SpawnableItems[0].Item, gameObject.transform.position + PerItemOffset * i,
                        gameObject.transform.rotation);
                    item.transform.SetParent(GameObject.Find("Environment").transform);
                    NetworkServer.Spawn(item);
                    if (Storage != null)
                    {
                        Storage.TransferResult result = Storage.AddItem(item);
                        switch (result)
                        {
                            case Storage.TransferResult.Success:
                                item.GetComponent<Item>().Visible = false;
                                //item.GetComponent<Item>().RpcSetVisibility(false);
                                break;
                            case Storage.TransferResult.SourceHasNoItem:
                                Debug.Log("Src has no item!");
                                break;
                            case Storage.TransferResult.NoFreeSpace:
                                Debug.Log(String.Format("No free space! Volume: {0}, Used: {1}, Free: {2}", Storage.MaxSlots, Storage.UsedSlots, Storage.FreeSpace));
                                break;
                            case Storage.TransferResult.UnsuitableItem:
                                Debug.Log("Unsuitable item");
                                break;
                            case Storage.TransferResult.TooLargeItem:
                                Debug.Log("Too large item");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    
                }
                
            }

            if(DestroyUponCall)
                Destroy(gameObject);
        }

        public void Spawn(bool destroy)
        {
            DestroyUponCall = destroy;
            Spawn();
        }
    }

}