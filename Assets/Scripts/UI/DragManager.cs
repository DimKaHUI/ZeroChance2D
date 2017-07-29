using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZeroChance2D.Assets.Scripts.Items;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.UI
{
    public interface IDropTarget
    {
        bool ItemDragEnd(GameObject dragGameObject);
    }

    public interface IDragTarget
    {
        GameObject DragGameObject { get; }
        void OnSuccessfullDrag();
    }

    public class DragManager : MonoBehaviour
    {

        public GameObject DragingGameObject;
        public RawImage DragingImage;
        public Vector2 DragingImageSize = new Vector2(55, 55);
        public float MinMouseOffset = 10f;
        [Range(5f, 255f)]
        public float ImageAlpha = 100f;

        private UIManager uiManager;
        private bool prevMouseState = false;
        private bool mousePressed = false;
        private Vector2 prevMousePos;
        private int equipmentSrc = -1;
        private IDragTarget uiDragTarget;

        void Start()
        {
            uiManager = gameObject.transform.parent.gameObject.GetComponent<UIManager>();
        }

        void Update()
        {
            mousePressed = Input.GetAxis("Fire and usage") > 0;
            if (!prevMouseState && mousePressed)
            {
                StartCoroutine(WaitForMouse(Input.mousePosition));
            }

            if(!mousePressed && DragingGameObject != null)
                EndDrag();

            prevMouseState = mousePressed;

            if (DragingGameObject != null)
            {
                Vector2 imagePos = Input.mousePosition;
                imagePos.x += DragingImage.rectTransform.sizeDelta.x / 2;
                imagePos.y -= DragingImage.rectTransform.sizeDelta.y / 2;
                DragingImage.rectTransform.anchoredPosition = imagePos;
            }
        }

        IEnumerator WaitForMouse(Vector2 curMousePos)
        {
            while (mousePressed)
            {
                if (Vector2.Distance(curMousePos, Input.mousePosition) >= MinMouseOffset)
                {
                    StartDrag(curMousePos);
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        void StartDrag(Vector2 elementPos)
        {
            uiDragTarget = uiManager.UnderPointUi(elementPos).FindComponentInParents<IDragTarget>();
            if (uiDragTarget != null)
                DragingGameObject = uiDragTarget.DragGameObject;


            if (DragingGameObject != null)
            {
                DragingImage.texture = DragingGameObject.GetComponent<SpriteRenderer>().sprite.texture;
                //DragingImage.color = new Color(255f, 255f, 255f, ImageAlpha);
                
                DragingImage.rectTransform.sizeDelta = DragingGameObject.GetComponent<SpriteRenderer>()
                    .sprite.texture.FitSize(new Vector2(DragingImageSize.x, DragingImageSize.y));
                DragingImage.gameObject.SetActive(true);
                DragingImage.gameObject.transform.SetAsLastSibling();
                
            }
        }

        void EndDrag()
        {
            DragingImage.texture = null;
            DragingImage.gameObject.SetActive(false);

            GameObject underPointUi = uiManager.UnderPointUi(Input.mousePosition);

            if (underPointUi == null)
            {
                uiManager.PlayerCtrl.PutItem(uiDragTarget.DragGameObject,
                    uiManager.PointToWorldLoc(Input.mousePosition));
                uiDragTarget.OnSuccessfullDrag();
            }
            else
            {
                IDropTarget ui = underPointUi.FindComponentInParents<IDropTarget>();
                if (ui != null)
                {
                    bool success = ui.ItemDragEnd(DragingGameObject);
                    if (success)
                    {
                        uiDragTarget.OnSuccessfullDrag();
                    }

                }
            }

            DragingGameObject = null;
        }


    }
}
