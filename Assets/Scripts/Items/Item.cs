using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(NetworkTransform))]
    public class Item : NetworkBehaviour
    {
        public int SlotSize;
        public float Weight;
        public string ItemName;
        [SyncVar]
        public GameObject User;

        public HandSide HandSide;
        //public string Description;
        [SyncVar]
        public DescriptionParameters DescriptionParameters;


        [SyncVar(hook = "Visualization")]
        public bool Visible = true;
        public virtual void Use(GameObject user, GameObject target = null)
        {
            
        }

        public virtual void Use(GameObject user, Vector2 targetPoint)
        {
            
        }

        public virtual void Use()
        {
            
        }

        [Client]
        void Visualization(bool Visible)
        {
            if (!Visible)
            {
                gameObject.layer = LayerMask.NameToLayer("Hidden");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Environment");
            }
        }
    }
}