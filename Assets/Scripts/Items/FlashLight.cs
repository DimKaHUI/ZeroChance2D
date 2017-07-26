using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(NetworkTransform))]
    //[NetworkSettings(channel = 2, sendInterval = 0.01f)]
    public class FlashLight : Item
    {
        [SyncVar]
        public bool TurnedOn = false;
        public float Range;
        public float VerticalOffset;

        public float LerpRate = 30;

        public Sprite StateOnSprite;
        public Sprite StateOffSprite;

        public AudioClip[] ClickSounds;

        public GameObject LightSourcePrefab;

        [SyncVar]
        public GameObject lightSource;
        [SyncVar]
        public Vector3 lightSourcePos;

        [SyncVar] private float lightSourceZRotation;

        public override void Use()
        {
            TurnedOn = !TurnedOn;

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
                    if (HandSide == HandSide.Left)
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

                lightSourcePos.z = -VerticalOffset;
                //lightSource 
                var light = Instantiate(LightSourcePrefab, lightSourcePos, Quaternion.Euler(0, 0, lightSourceZRotation));
                NetworkServer.Spawn(light);
                lightSource = light;
            }
            else
            {
                NetworkServer.Destroy(lightSource);
            }
        }

        [Server]
        void Update()
        {
            if (User == null)
            {
                lightSourcePos = gameObject.transform.position;
                lightSourceZRotation = gameObject.transform.rotation.eulerAngles.z + 90;
            }
            else if (User.GetComponent<Human>() != null)
            {
                GameObject handObj;
                if (HandSide == HandSide.Left)
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

            lightSourcePos.z = VerticalOffset;
        }

        [Client]
        void FixedUpdate()
        {
            if (lightSource != null)
            {
                if (User == null)
                {
                    lightSource.transform.position = Vector3.Lerp(lightSource.transform.position, lightSourcePos,
                        LerpRate * Time.deltaTime);
                    lightSource.transform.rotation = Quaternion.Lerp(lightSource.transform.rotation,
                        Quaternion.Euler(0, 0, lightSourceZRotation),
                        LerpRate * Time.deltaTime);
                }
                else
                {
                    if (HandSide == HandSide.Left)
                    {
                        var pos = User.transform.Find("LeftHandPoint").position;
                        pos.z = VerticalOffset;
                        lightSource.transform.position = pos;
                        lightSource.transform.rotation = User.transform.Find("LeftHandPoint").rotation;
                    }
                    else
                    {
                        var pos = User.transform.Find("RightHandPoint").position;
                        pos.z = VerticalOffset;
                        lightSource.transform.position = pos;
                        lightSource.transform.rotation = User.transform.Find("RightHandPoint").rotation;
                    }
                }
            }

            // Sprite changing
            if (TurnedOn)
                gameObject.GetComponent<SpriteRenderer>().sprite = StateOnSprite;
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = StateOffSprite;
            }
        }

    }
}