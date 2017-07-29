using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZeroChance2D.Assets.Scripts.Items;
using ZeroChance2D.Assets.Scripts.Mechanics;
using ZeroChance2D.Assets.Scripts.UI;

namespace ZeroChance2D.Assets.Scripts
{

    public class CrateExplorer : MonoBehaviour, IDropTarget
    {
        public GameObject ItemButtonPrefab;
        public GameObject User;
        public Storage AttachedStorage;
        public Transform ContentField;
        public float ClosingDelay = 0.5f;
        public float UiUpdatedCooldown = 0.02f;

        private bool canClose = false;

        private List<GameObject> buttons = new List<GameObject>();
        private UIManager uiManager;



        // Use this for initialization
        void Start()
        {
            uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            uiManager.MovableUI = gameObject;
            StartCoroutine(wait());
            StartCoroutine(UiUpdater(UiUpdatedCooldown));
            var addItemButt = gameObject.transform.Find("AddItemButton").gameObject.GetComponent<Button>();
            addItemButt.onClick.AddListener(AddItemClick);
            SetupUI();
        }

        void WhileDragging()
        {
            if (uiManager.DragManager.GetComponent<DragManager>().DragingGameObject != null)
            {
                foreach (var butt in buttons)
                {
                    if (butt != null)
                    butt.GetComponent<CExpButton>().SwitchRaycast(false);
                }
            }
            else
            {
                foreach (var butt in buttons)
                {
                    if(butt != null)
                    butt.GetComponent<CExpButton>().SwitchRaycast(false);
                }
            }
        }

        void SetupUI()
        {
            for (int i = 0; i < AttachedStorage.Inventory.StoredList.Length; i++)
            {
                var itemObj = AttachedStorage.Inventory.StoredList[i];
                CreateNewButton(itemObj);
            }
        }

        void UpdateUI()
        {
            // Deleting buttons linked to null items
            foreach (var butt in buttons)
            {
                if(butt != null)
                if(AttachedStorage.Inventory.IndexOf(butt.GetComponent<CExpButton>().ItemObj) == -1)
                    Destroy(butt);
            }
            // Creating new buttons
            foreach (var item in AttachedStorage.Inventory.StoredList)
            {
                if (!IsInButtons(item))
                {
                    CreateNewButton(item);
                }
            }
        }

        bool IsInButtons(GameObject item)
        {
            foreach (var butt in buttons)
            {
                if(butt == null)
                    continue;
                
                if (butt.GetComponent<CExpButton>().ItemObj == item)
                    return true;
            }
            return false;
        }

        void CreateNewButton(GameObject itemObj)
        {
            var button = Instantiate(ItemButtonPrefab);
            button.transform.SetParent(ContentField);
            button.transform.Find("Text").gameObject.GetComponent<Text>().text =
                itemObj.GetComponent<Item>().ItemName;
            button.GetComponent<CExpButton>().Explorer = this;
            button.GetComponent<CExpButton>().ItemObj = itemObj;
            button.GetComponent<Button>()
                .onClick.AddListener(delegate { button.GetComponent<CExpButton>().Click(); });
            var image = button.transform.Find("Image").gameObject.GetComponent<RawImage>();
            image.texture = itemObj.GetComponent<SpriteRenderer>().sprite.texture;
            image.rectTransform.sizeDelta = itemObj.GetComponent<SpriteRenderer>()
                .sprite.texture.FitSize(image.rectTransform.sizeDelta);
            image.color = new Color(255f, 255f, 255f, 255f);
            buttons.Add(button);
        }

        void ClearUI()
        {
            foreach (var butObj in buttons)
                Destroy(butObj);
        }

        public void ItemCallback(GameObject button, GameObject itemObj)
        {
            var controller = User.GetComponent<PlayerController>();
            if (User.GetComponent<Human>().Equipment[(int) controller.ActiveHand] == null)
            {
                User.GetComponent<Human>().Equipment[(int) controller.ActiveHand] = itemObj;
                controller.CmdRemoveFromStorage(AttachedStorage.gameObject, itemObj);
                UpdateUI();
            }
            else
            {
                Debug.Log("Hand was not empty!");
            }
        }

        public void DropEndCallback(GameObject button, GameObject itemObj)
        {
            var controller = User.GetComponent<PlayerController>();
            controller.CmdRemoveFromStorage(AttachedStorage.gameObject, itemObj);
            UpdateUI();
        }

        private bool prevMouseState = false;
        void FixedUpdate()
        {
            bool mousePressed = Input.GetAxis("Fire and usage") > 0;
            if (canClose)
            {
                UIManager ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
                if (!prevMouseState && mousePressed && ui.UiMask == ui.CursorUi())
                {
                    ui.UiMask.GetComponent<Image>().raycastTarget = false;
                    Destroy(gameObject);
                }
            }
            prevMouseState = mousePressed;
        }

        IEnumerator wait()
        {
            yield return new WaitForSeconds(ClosingDelay);
            canClose = true;
        }

        public void AddItemClick()
        {
            GameObject item;
            int equipmentPos = -1;
            switch (User.GetComponent<PlayerController>().ActiveHand)
            {
                case HandSide.Left:
                    item = User.GetComponent<Human>().Equipment[0];
                    if (item != null)
                    {
                        equipmentPos = 0;
                    }
                    break;
                case HandSide.Right:
                    item = User.GetComponent<Human>().Equipment[1];
                    if (item != null)
                    {
                        equipmentPos = 1;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (equipmentPos != -1)
            {
                User.GetComponent<PlayerController>().CmdAddToStorage(AttachedStorage.gameObject, item);
                User.GetComponent<Human>().Equipment[equipmentPos] = null;
                UpdateUI();
            }
            else
            {
                Debug.Log("Hand is empty!");
            }
        }

        IEnumerator UiUpdater(float cooldown)
        {
            while (true)
            {
                yield return new WaitForSeconds(cooldown);
                UpdateUI();
            }
        }

        // TODO Retarded code. Cure it pls
        public bool ItemDragEnd(GameObject dragGameObject)
        {
            if (dragGameObject.GetComponent<Item>() != null)
            {
                if (AttachedStorage.AddItem(dragGameObject) == Storage.TransferResult.Success)
                {
                    User.GetComponent<PlayerController>().CmdAddToStorage(AttachedStorage.gameObject, dragGameObject);
                    return true;
                }

            }
            return false;
        }

        void OnDestroy()
        {
            if (uiManager.MovableUI == gameObject)
                uiManager.MovableUI = null;
        }

    }
}
