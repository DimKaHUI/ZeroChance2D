using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    
    public class House : MonoBehaviour
    {
        public Vector2 Size;
        // Has spawner
        // Spawn positions
        // Item spawners
    }

    public class LandscapeGenerator : NetworkBehaviour
    {
        public Vector2 MapSize;
        [Tooltip("If true, all Item spawners will be forced to be destroy after Spawn() invokation")]
        public bool ForceDestroySpawners;

        public GameObject[] HousePrefabs;
        public GameObject[] RoadPrefab = new GameObject[2];
        public GameObject[] SmallRoadPrefab = new  GameObject[2];
        public override void OnStartServer()
        {
            SpawnItems();
        }

        public void SpawnItems()
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("ItemSpawner");
            if(spawners.Length == 0)
                Debug.LogWarning("No spawners found!");
            else
            {
                foreach (var spawner in spawners)
                {
                    if(ForceDestroySpawners)
                        spawner.GetComponent<ItemSpawner>().Spawn(true);
                    else
                    {
                        spawner.GetComponent<ItemSpawner>().Spawn();
                    }
                }
            }
        }

    }
}
