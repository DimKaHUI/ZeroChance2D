using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZeroChance2D
{
    [Serializable]
    public struct DescriptionParameters
    {
        public Vector2 WrappedSize;
        public Vector2 UnwrappedSize;
        public string Description;
        public string ExternalItemName;
    }

    public class DescriptionPanel : MonoBehaviour
    {
        public GameObject DescriptionPanelObject;
        public Text NameBox;
        public Text DescriptionBox;

        public float DescriptionUnfadingDelay = 1.5f;
        public float UnfadingAnimationTime = 0.3f;

        [HideInInspector]
        public bool Active;

        private GameObject itemGameObject;
        private Item item;

        public GameObject ItemGameObject
        {
            get { return itemGameObject; }
            set
            {
                if (value != null)
                {
                    if (itemGameObject != value)
                    {
                        NameBox.text = " " + value.GetComponent<Item>().DescriptionParameters.ExternalItemName;
                        DescriptionBox.text = value.GetComponent<Item>().DescriptionParameters.Description;
                        itemGameObject = value;
                        Active = true;

                        FadeDescription();
                        StartCoroutine(UnfadeDescription());
                    }
                }
                else
                {
                    StopAllCoroutines();
                    Active = false;
                    itemGameObject = null;
                }
                

            }
        }

        void Update()
        {
            if (Active)
            {
                DescriptionPanelObject.SetActive(true);
                Vector2 panelpos = Input.mousePosition;
                panelpos.x += DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta.x / 2;
                panelpos.y += DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta.y / 2;
                DescriptionPanelObject.GetComponent<RectTransform>().anchoredPosition = panelpos;
                NameBox.text = itemGameObject.GetComponent<Item>().DescriptionParameters.ExternalItemName;
                DescriptionBox.text = itemGameObject.GetComponent<Item>().DescriptionParameters.Description;

                /*UnwrappedSize.x = DescriptionBox.preferredWidth + 10;
                UnwrappedSize.y = DescriptionBox.preferredHeight + NameBox.gameObject.GetComponent<RectTransform>().sizeDelta.y + 10;
                WrappedSize.x = NameBox.preferredWidth + 5;
                WrappedSize.y = NameBox.preferredHeight + 5;*/
            }
            else
            {
                DescriptionPanelObject.SetActive(false);
            }
        }

        void Start()
        {
            Active = false;
        }


        public void FadeDescription()
        {
            DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta = itemGameObject.GetComponent<Item>().DescriptionParameters.WrappedSize;
            DescriptionBox.gameObject.SetActive(false);
        }

        public IEnumerator UnfadeDescription()
        {
            yield return new WaitForSeconds(DescriptionUnfadingDelay);
            StartCoroutine(UnfadeAnimation());
        }

        public IEnumerator UnfadeAnimation()
        {
            Vector2 delta = (itemGameObject.GetComponent<Item>().DescriptionParameters.UnwrappedSize - itemGameObject.GetComponent<Item>().DescriptionParameters.WrappedSize) / UnfadingAnimationTime;

            while (DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta.x < itemGameObject.GetComponent<Item>().DescriptionParameters.UnwrappedSize.x && DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta.y < itemGameObject.GetComponent<Item>().DescriptionParameters.UnwrappedSize.y)
            {
                DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta += delta * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta = itemGameObject.GetComponent<Item>().DescriptionParameters.UnwrappedSize;
            DescriptionBox.gameObject.SetActive(true);
            DescriptionBox.text = itemGameObject.GetComponent<Item>().ItemName;
        }

    }
}