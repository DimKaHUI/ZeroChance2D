using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    
    public class Weapon : MonoBehaviour
    {
        public WeaponModels WeaponModel;
        public enum WeaponShootingMode{Auto, SemiAuto}
        public int ShootsPerMinute;
        public WeaponShootingMode ShootingMode;
        public AmmoClip InsertedClip;
        public AudioClip[] ShootAudioClips;
        public AudioClip ReloadAudioClip;

        public float ScatterAngle;
        public float InitialBulletSpeed;
        
    }
}