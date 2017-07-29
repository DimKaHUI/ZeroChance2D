using System;
using UnityEngine;
using UnityEngine.Networking;
using ZeroChance2D.Assets.Scripts.Mechanics;
using ZeroChance2D.Assets.Scripts.UI;

namespace ZeroChance2D.Assets.Scripts.Items
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

        [SyncVar]
        public HandSide HandSide;

        [SyncVar]
        public DescriptionParameters DescriptionParameters;

        public bool Visible = true;
        public virtual void Use(GameObject user, GameObject target = null)
        {
            
        }

        public virtual void Use(GameObject user, Vector2 targetPoint)
        {

        }

        public virtual void Use()
        {

            string user = User.name;
            
            Debug.Log(String.Format("User: {0}, Item name: {1}, Hand: {2}", user, ItemName, HandSide));
        }


        void Update()
        {
            Visualization();
        }

        public void Visualization()
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