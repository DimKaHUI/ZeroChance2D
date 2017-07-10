using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ZeroChance2D
{

    public enum HandSide { Left, Right}
    public class UIManger : NetworkBehaviour
    {
        public Image LeftHandImage;
        public Image RightHandImage;
        public Image HealthIndicator;
        public HandSide ActiveHand;
        public GameObject Player;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {

        }
    }
}