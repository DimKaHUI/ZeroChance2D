using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    
    public class Weapon : NetworkBehaviour
    {
        public WeaponModels WeaponModel;
        public GameObject BulletPrefab;
        public enum WeaponShootingMode{Auto, SemiAuto}
        public float CooldownDuration;
        public WeaponShootingMode ShootingMode;
        public AmmoClip InsertedClip;
        public AudioClip[] ShootAudioClips;
        public AudioClip ReloadAudioClip;

        public float ScatterAngle;
        public float InitialBulletSpeed;
        public float BulletTimeout;

        public bool ReadyToShoot = true;

        public IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(CooldownDuration);
            ReadyToShoot = true;
        }

        [Command]
        public void CmdUse(GameObject user)
        {
            var bullet = Instantiate(BulletPrefab, user.transform.position, user.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = user.transform.up * InitialBulletSpeed;
            bullet.GetComponent<Bullet>().Lifetime = BulletTimeout;
            NetworkServer.Spawn(bullet);
            Debug.Log("Shoot!");
            ReadyToShoot = false;
            StartCoroutine(Cooldown());
        }
        
    }
}