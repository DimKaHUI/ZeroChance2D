using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZeroChance2D
{

    public class DescriptionPanel : MonoBehaviour
    {
        public GameObject DescriptionPanelObject;
        public Text NameBox;
        public Text DescriptionBox;

        public float DescriptionUnfadingDelay = 1.5f;
        public float UnfadingAnimationTime = 1f;

        public Vector2 WrappedSize;
        [HideInInspector]
        public Vector2 UnwrappedSize;
        //[HideInInspector]
        public bool Active;

        public GameObject itemGameObject;
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
                        Active = true;
                        FadeDescription();
                        StartCoroutine(UnfadeDescription());
                    }
                }
                else
                {
                    StopAllCoroutines();
                    Active = false;
                }
                itemGameObject = value;

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
                NameBox.text = itemGameObject.GetComponent<Item>().ItemName;
                DescriptionBox.text = itemGameObject.GetComponent<Item>().Description;
            }
            else
            {
                DescriptionPanelObject.SetActive(false);
            }
        }

        void Start()
        {
            UnwrappedSize = DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta;
            Active = false;
        }


        public void FadeDescription()
        {
            DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta = WrappedSize;
            DescriptionBox.gameObject.SetActive(false);
        }

        public IEnumerator UnfadeDescription()
        {
            yield return new WaitForSeconds(DescriptionUnfadingDelay);
            StartCoroutine(UnfadeAnimation());
        }

        public IEnumerator UnfadeAnimation()
        {
            Vector2 delta = (UnwrappedSize - WrappedSize)  / UnfadingAnimationTime;

            while (DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta.x <= UnwrappedSize.x && DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta.y <= UnwrappedSize.y)
            {
                DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta += delta * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            DescriptionPanelObject.GetComponent<RectTransform>().sizeDelta = UnwrappedSize;
            DescriptionBox.gameObject.SetActive(true);
        }

    }
}