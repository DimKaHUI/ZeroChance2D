using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts
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
