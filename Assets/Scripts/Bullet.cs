using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : NetworkBehaviour
    {
        [SyncVar]
        public float Speed;

        [SyncVar]
        public float PerSecondSpeedDecrement;
        [SyncVar]
        public Damage BaseDamage;
        [SyncVar]
        public float Lifetime;

        // Use this for initialization
        void Start()
        {
            if (Lifetime > 0)
                StartCoroutine(Timeout());
        }

        public IEnumerator Timeout()
        {
            yield return new WaitForSeconds(Lifetime);
            Destroy(gameObject);
        }
    }
}
