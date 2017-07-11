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
        public Clip InsertedClip;

        public float ScatterAngle;
        /// <summary>
        /// Bullet speed in meters per second
        /// </summary>
        public float InitialBulletSpeed;
        
    }
}