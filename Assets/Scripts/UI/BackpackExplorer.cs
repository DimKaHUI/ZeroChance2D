using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZeroChance2D.Assets.Scripts.Items;
using ZeroChance2D.Assets.Scripts.Mechanics;

namespace ZeroChance2D.Assets.Scripts.UI
{

    public class BackpackExplorer : MonoBehaviour, IStorageUi
    {
        public GameObject ItemButtonPrefab;

        public GameObject User { get; set; }
        public Storage AttachedStorage { get; set; }
        public float UiUpdatedCooldown = 0.02f;

        private UIManager _uiManager;
        private List<GameObject> buttons = new List<GameObject>();
        public Transform ContentField;

        void Start()
        {
            _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            _uiManager.MovableUI = gameObject;
            StartCoroutine(UiUpdater(UiUpdatedCooldown));
        }

        public bool ItemDragEnd(GameObject dragGameObject)
        {
            if (dragGameObject.GetComponent<Item>() != null)
            {
                User.GetComponent<PlayerController>()
                    .CmdPutIntoStorage(AttachedStorage.gameObject,
                        _uiManager.PlayerHuman.Equipment.IndexOf(dragGameObject));

            }
            return false;
        }

        IEnumerator UiUpdater(float cooldown)
        {
            while (true)
            {
                yield return new WaitForSeconds(cooldown);
                UpdateUI();
            }
        }

        void UpdateUI()
        {
            // Deleting buttons linked to null items
            foreach (var butt in buttons)
            {
                if (butt != null)
                    if (AttachedStorage.Inventory.IndexOf(butt.GetComponent<BExpButton>().ItemObj) == -1)
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
                if (butt == null)
                    continue;

                if (butt.GetComponent<BExpButton>().ItemObj == item)
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
            button.GetComponent<BExpButton>().Explorer = this;
            button.GetComponent<BExpButton>().ItemObj = itemObj;
            button.GetComponent<Button>()
                .onClick.AddListener(delegate { button.GetComponent<BExpButton>().Click(); });
            var image = button.transform.Find("Image").gameObject.GetComponent<RawImage>();
            image.texture = itemObj.GetComponent<SpriteRenderer>().sprite.texture;
            image.rectTransform.sizeDelta = itemObj.GetComponent<SpriteRenderer>()
                .sprite.texture.FitSize(image.rectTransform.sizeDelta);
            image.color = new Color(255f, 255f, 255f, 255f);
            buttons.Add(button);
        }

        public void ItemCallback(GameObject button, GameObject itemObj)
        {
            var controller = User.GetComponent<PlayerController>();
            if (User.GetComponent<Human>().Equipment[(int)controller.ActiveHand] == null)
            {
                //User.GetComponent<Human>().Equipment[(int) controller.ActiveHand] = itemObj;
                controller.CmdSetupEquipment((int)controller.ActiveHand, itemObj);
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

    }
}
