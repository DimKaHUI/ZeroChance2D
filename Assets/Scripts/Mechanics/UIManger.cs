using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ZeroChance2D
{
    public class UIManger : MonoBehaviour
    {
        public Image LeftHandImage;
        public Image RightHandImage;
        public RawImage LeftItemImage;
        public RawImage RightItemImage;
        public Image HealthIndicator;

        private GameObject playerObj;
        private Human playerHuman;
        private PlayerController playerController;

        public GameObject LocalPlayerObject
        {
            get { return playerObj; }
            set
            {
                playerObj = value;
                playerHuman = playerObj.GetComponent<Human>();
                playerController = playerObj.GetComponent<PlayerController>();
            }
        }

        public Vector2 UnderCursorPoint
        {
            get
            {
                var clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPoint.z = 0;
                return clickPoint;
            }
        }
        public GameObject UnderCursor(string layer)
        {
            var hitItem = Physics2D.Raycast(UnderCursorPoint, new Vector2(0, 0), Mathf.Infinity,
                LayerMask.NameToLayer(layer));
            return hitItem.collider == null ? null : hitItem.collider.gameObject;
        }

        public bool IsCursorUponUi()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                foreach (var res in results)
                {
                    if (res.gameObject.layer == LayerMask.NameToLayer("UI"))
                        return true;
                }
            }
            return false;
        }

        void Update()
        {
            if (playerHuman == null)
            {
                Debug.LogWarning("No player attached!");
                return;
            }
            // Drawing sprites
            if (playerHuman.Equipment.LeftHandItem != null)
            {
                var image = playerHuman.Equipment.LeftHandItem.GetComponent<SpriteRenderer>()
                    .sprite.texture;
                var imageSize = new Vector2(image.width, image.height);

                if (imageSize.x > imageSize.y)
                {
                    float coef = imageSize.y / imageSize.x;
                    LeftItemImage.rectTransform.sizeDelta = new Vector2(55, 55 * coef);
                }
                else
                {
                    float coef = imageSize.x / imageSize.y;
                    LeftItemImage.rectTransform.sizeDelta = new Vector2(55 * coef, 55);
                }

                LeftItemImage.texture = image;
            }
            else
            {
                LeftItemImage.texture = null;
            }


            if (playerHuman.Equipment.RightHandItem != null)
            {
                var image = playerHuman.Equipment.RightHandItem.GetComponent<SpriteRenderer>()
                    .sprite.texture;
                var imageSize = new Vector2(image.width, image.height);

                if (imageSize.x > imageSize.y)
                {
                    float coef = imageSize.y / imageSize.x;
                    RightItemImage.rectTransform.sizeDelta = new Vector2(55, 55 * coef);
                }
                else
                {
                    float coef = imageSize.x / imageSize.y;
                    RightItemImage.rectTransform.sizeDelta = new Vector2(55 * coef, 55);
                }

                RightItemImage.texture = image;

            }
            else
                RightItemImage.texture = null;
        }
        public void SelectHand(bool RightSide)
        {
            if (RightSide)
                playerController.ActiveHand = HandSide.Right;
            else
            {
                playerController.ActiveHand = HandSide.Left;
            }
        }
    }
}