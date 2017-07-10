using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{

    public class PlayerController : Human
    {
        public float RotationSpeed = 60f;
        private UIManger uiManger;

        void Start()
        {
            uiManger = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManger>();
            uiManger.AttachedController = this;
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
