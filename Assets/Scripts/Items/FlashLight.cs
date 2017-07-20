using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    //[RequireComponent(typeof(Light))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(NetworkTransform))]
    public class FlashLight : Item
    {
        public bool TurnedOn = false;
        public float Range;
        public float VerticalOffset;

        public Sprite StateOnSprite;
        public Sprite StateOffSprite;

        public AudioClip[] ClickSounds;

        public GameObject LightSourcePrefab;

        private GameObject lightSource;
        [SyncVar]
        private Vector2 lightSourcePos;

        [SyncVar] private float lightSourceZRotation;

        public override void Use()
        {
            CmdUse();
        }

        [Command]
        void CmdUse()
        {
            TurnedOn = !TurnedOn;
        }

        void Start()
        {

        }


        void FixedUpdate()
        {
            if (TurnedOn)
            {
                if (User == null)
                {
                    lightSourcePos = gameObject.transform.position;
                    lightSourceZRotation = gameObject.transform.rotation.eulerAngles.z;
                }
                else if (User.GetComponent<Human>() != null)
                {
                    GameObject handObj;
                    if(HandSide == HandSide.Left)
                        handObj = User.transform.Find("LeftHandPoint").gameObject;
                    else
                        handObj = User.transform.Find("RightHandPoint").gameObject;
                    
                    lightSourcePos = handObj.transform.position;
                    lightSourceZRotation = handObj.transform.rotation.eulerAngles.z;
                }
                else
                {
                    // TODO If not a human
                    throw new NotImplementedException();
                }

                //lightSource 
            }
        }
    }
}