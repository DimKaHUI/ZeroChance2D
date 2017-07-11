using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    [RequireComponent(typeof(Collider2D))]
    public class Item : MonoBehaviour
    {
        public int SlotSize;
        public float Weight;
        public string ItemName;

        public virtual void Use(GameObject user, GameObject target = null)
        {
            
        }

        public virtual void Use(GameObject user, Vector2 targetPoint)
        {
            
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}