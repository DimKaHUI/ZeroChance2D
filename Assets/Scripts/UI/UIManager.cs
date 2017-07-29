using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.UI
{
    public static class Extentions
    {
        public static Vector2 FitSize(this Texture2D image, Vector2 size)
        {

            var imageSize = new Vector2(image.width, image.height);

            if (imageSize.x > imageSize.y)
            {
                float coef = imageSize.y / imageSize.x;
                return new Vector2(size.x, size.y * coef);
            }
            else
            {
                float coef = imageSize.x / imageSize.y;
                return new Vector2(size.x * coef, size.y);
            }


        }

        public static T FindComponentInParents<T>(this GameObject child) where T:class
        {
            if (child.GetComponent<T>() != null)
                return child.GetComponent<T>();
            if (child.transform.parent == null)
                return null;
            return FindComponentInParents<T>(child.transform.parent.gameObject);
        }
    }

    public class UIManager : MonoBehaviour
    {
        public GameObject LeftHandObj;
        public GameObject RightHandObj;
        public Sprite LeftHandNotActiveSprite;
        public Sprite LeftHandActiveSprite;
        public Sprite RightHandNotActiveSprite;
        public Sprite RightHandActiveSprite;
        public RawImage LeftItemImage;
        public RawImage RightItemImage;
        public Image HealthIndicator;

        public GameObject DescriptionPanelController;
        public GameObject UiMask;
        public GameObject MovableUI;
        public GameObject DragManager;
       
        private GameObject playerObj;
        public Human PlayerHuman;
        [HideInInspector]
        public PlayerController PlayerCtrl;

        public GameObject LocalPlayerObject
        {
            get { return playerObj; }
            set
            {
                playerObj = value;
                PlayerHuman = playerObj.GetComponent<Human>();
                PlayerCtrl = playerObj.GetComponent<PlayerController>();
            }
        }

        [Obsolete("Use PointToWorldLoc(Vector2)")]
        public Vector2 UnderCursorPoint
        {
            get
            {
                var clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPoint.z = 0;
                return clickPoint;
            }
        }

        public Vector2 PointToWorldLoc(Vector2 point)
        {
            var clickPoint = Camera.main.ScreenToWorldPoint(point);
            clickPoint.z = 0;
            return clickPoint;
        }

        [Obsolete("Use UnderPoint(Vector2) instead")]
        public GameObject UnderCursor(string layer)
        {
            var hitItem = Physics2D.Raycast(UnderCursorPoint, new Vector2(0, 0), Mathf.Infinity,
            LayerMask.NameToLayer(layer));

            var hitCollider = hitItem.collider;
            if (hitCollider == null)
                return null;
            if (hitCollider.gameObject.GetComponent<SpriteRenderer>() == null)
                return null;
            if (hitCollider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == layer)
                return hitCollider.gameObject;
            return null;
        }
        public GameObject UnderPoint(string layer, Vector2 pos)
        {
            var hitItem = Physics2D.Raycast(pos, new Vector2(0, 0), Mathf.Infinity,
                LayerMask.NameToLayer(layer));

            var hitCollider = hitItem.collider;
            if (hitCollider == null)
                return null;
            if (hitCollider.gameObject.GetComponent<SpriteRenderer>() == null)
                return null;
            if (hitCollider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == layer)
                return hitCollider.gameObject;
            return null;
        } 

        [Obsolete("Use UnderPointUi(Vector2) instead")]
        public GameObject CursorUi()
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
                        return res.gameObject;
                }
            }
            return null;
        }

        public GameObject UnderPointUi(Vector2 pos)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = pos;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                foreach (var res in results)
                {
                    if (res.gameObject.layer == LayerMask.NameToLayer("UI"))
                        return res.gameObject;
                }
            }
            return null;
        }

        [Obsolete("Use IsPointUponUi(Vector2) instead")]
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

        public bool IsPointUponUi(Vector2 point)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = point;

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
            if (PlayerHuman == null)
            {
                return;
            }

            // Showing description panel
            if (PlayerCtrl.ManipulatedItem != null)
            {
                DescriptionPanelController.GetComponent<DescriptionPanel>().ItemGameObject = PlayerCtrl.ManipulatedItem;
            }
            else
            {
                DescriptionPanelController.GetComponent<DescriptionPanel>().ItemGameObject = null;
            }

            // Drawing sprites
            // TODO Remove magic numbers
            if (PlayerHuman.Equipment[Equipment.EquipmentSlot.LeftHand] != null)
            {
                var image = PlayerHuman.Equipment[Equipment.EquipmentSlot.LeftHand].GetComponent<SpriteRenderer>()
                    .sprite.texture;
                LeftItemImage.rectTransform.sizeDelta = image.FitSize(new Vector2(55, 55));

                LeftItemImage.texture = image;
                LeftItemImage.color = new Color(LeftItemImage.color.r, LeftItemImage.color.g, LeftItemImage.color.b, 255f);
            }
            else
            {
                LeftItemImage.texture = null;
                LeftItemImage.color = new Color(LeftItemImage.color.r, LeftItemImage.color.g, LeftItemImage.color.b, 0);
            }


            if (PlayerHuman.Equipment[Equipment.EquipmentSlot.RightHand] != null)
            {
                var image = PlayerHuman.Equipment[Equipment.EquipmentSlot.RightHand]
                    .GetComponent<SpriteRenderer>()
                    .sprite.texture;
                
                RightItemImage.rectTransform.sizeDelta = image.FitSize(new Vector2(55, 55));

                RightItemImage.texture = image;
                RightItemImage.color = new Color(RightItemImage.color.r, RightItemImage.color.g, RightItemImage.color.b, 255f);

            }
            else
            {
                RightItemImage.texture = null;
                RightItemImage.color = new Color(RightItemImage.color.r, RightItemImage.color.g, RightItemImage.color.b, 0);
            }

            // hand sprites swaping
            switch (PlayerCtrl.ActiveHand)
            {
                case HandSide.Left:
                    LeftHandObj.GetComponent<Image>().sprite = LeftHandActiveSprite;
                    RightHandObj.GetComponent<Image>().sprite = RightHandNotActiveSprite;
                    break;
                case HandSide.Right:
                    LeftHandObj.GetComponent<Image>().sprite = LeftHandNotActiveSprite;
                    RightHandObj.GetComponent<Image>().sprite = RightHandActiveSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void SelectHand()
        {
            if (PlayerCtrl == null)
                return;
            if (PlayerCtrl.ActiveHand == HandSide.Left)
                PlayerCtrl.ActiveHand = HandSide.Right;
            else
            {
                PlayerCtrl.ActiveHand = HandSide.Left;
            }
        }

        public void HandledItemClick(bool rightSide)
        {
            PlayerCtrl.CmdHandledItemClick(playerObj, rightSide);
        }
    }
}