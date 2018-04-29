using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ZeroChance2D.Assets.Scripts.UI
{

    public class BExpButton : MonoBehaviour, IDragTarget
    {
        public IStorageUi Explorer;
        public GameObject ItemObj;

        public GameObject DragGameObject
        {
            get { return ItemObj; }
        }

        public void OnSuccessfullDrag()
        {
            Explorer.DropEndCallback(gameObject, ItemObj);
        }

        public void Click()
        {
            Explorer.ItemCallback(gameObject, ItemObj);
        }

        public void SwitchRaycast(bool isTarget)
        {
            //gameObject.transform.Find("Text").gameObject.GetComponent<Image>().raycastTarget = isTarget;
            gameObject.GetComponent<Image>().raycastTarget = isTarget;
            //gameObject.transform.Find("Image").gameObject.GetComponent<RawImage>().raycastTarget = isTarget;
        }
    }
}