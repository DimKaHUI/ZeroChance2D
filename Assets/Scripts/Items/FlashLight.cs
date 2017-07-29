using System;
using UnityEngine;
using UnityEngine.Networking;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.Items
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(NetworkTransform))]
    //[NetworkSettings(channel = 2, sendInterval = 0.01f)]
    public class FlashLight : Item
    {
        [SyncVar]
        public bool TurnedOn = false;
        // TODO DELETE UNUSED
        public float Range;
        public float VerticalOffset;
        public float IntensityWhileKept;

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

        public Texture defaultCookie;
        public float defaultIntensity;

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
        void UpdateLight()
        {
            if (User == null)
            {
                if (lightSource != null)
                {
                    lightSource.GetComponent<Light>().cookie = defaultCookie;
                    lightSource.GetComponent<Light>().intensity = defaultIntensity;
                }
                lightSourcePos = gameObject.transform.position;
                lightSourceZRotation = gameObject.transform.rotation.eulerAngles.z + 90;
            }
            else if (User.GetComponent<Storage>() != null)
            {
                if (lightSource != null)
                {
                    lightSource.GetComponent<Light>().cookie = null;
                    lightSource.GetComponent<Light>().intensity = IntensityWhileKept;
                }
            }
            else if (User.GetComponent<Human>() != null)
            {
                if (lightSource != null)
                {
                    lightSource.GetComponent<Light>().cookie = defaultCookie;
                    lightSource.GetComponent<Light>().intensity = defaultIntensity;
                }

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

        
        void Update()
        {
            Visualization();
            UpdateLight();
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
                else if(User.GetComponent<Human>() != null)
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
                else if (User.GetComponent<Storage>() != null)
                {
                    var pos = User.transform.position;
                    pos.z = VerticalOffset;
                    lightSource.transform.position = pos;
                    lightSource.transform.rotation = User.transform.rotation;
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