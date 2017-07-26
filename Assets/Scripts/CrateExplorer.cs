using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ZeroChance2D
{

    public class CrateExplorer : MonoBehaviour
    {
        public GameObject ItemButtonPrefab;
        public GameObject User;
        public Storage AttachedStorage;
        public float ClosingDelay = 0.5f;

        private bool canClose = false;

        private List<Button> buttons = new List<Button>();


        // Use this for initialization
        void Start()
        {
            StartCoroutine(wait());
            //foreach (var itemObj in storage.StoredList)
            for (int i = 0; i < AttachedStorage.Inventory.StoredList.Length; i++)
            {
                var itemObj = AttachedStorage.Inventory.StoredList[i];
                var button = Instantiate(ItemButtonPrefab);
                button.transform.SetParent(gameObject.transform);
                button.transform.Find("Text").gameObject.GetComponent<Text>().text = itemObj.GetComponent<Item>().ItemName;
                button.GetComponent<CExpButton>().Explorer = this;
                button.GetComponent<CExpButton>().ItemObj = itemObj;
                button.GetComponent<Button>().onClick.AddListener(delegate { button.GetComponent<CExpButton>().Click(); });
                var image = button.transform.Find("Image").gameObject.GetComponent<RawImage>();
                image.texture = itemObj.GetComponent<SpriteRenderer>().sprite.texture;
                image.rectTransform.sizeDelta = ResizedSizeDelta(itemObj.GetComponent<SpriteRenderer>().sprite.texture,
                    image.rectTransform.sizeDelta);
                image.color = new Color(255f, 255f, 255f, 255f);
            }
        }

        Vector2 ResizedSizeDelta(Texture2D image, Vector2 size)
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
                return  new Vector2(55 * coef, 55);
            }
        }

        public void ItemCallback(GameObject button, GameObject itemObj)
        {
            var controller = User.GetComponent<PlayerController>();
            if (User.GetComponent<Human>().Equipment[(int) controller.ActiveHand] == null)
            {
                User.GetComponent<Human>().Equipment[(int) controller.ActiveHand] = itemObj;
                controller.CmdRemoveFromStorage(AttachedStorage.gameObject, itemObj);
                Destroy(button);
            }
            else
            {
                Debug.Log("Hand was not empty!");
            }
        }

        void FixedUpdate()
        {
            if (canClose)
            {
                UIManger ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManger>();
                if (Input.GetAxis("Fire and usage") > 0 && ui.UiMask == ui.CursorUi())
                {
                    ui.UiMask.GetComponent<Image>().raycastTarget = false;
                    Destroy(gameObject);
                }
            }
        }

        IEnumerator wait()
        {
            yield return new WaitForSeconds(ClosingDelay);
            canClose = true;
        }
    }
}