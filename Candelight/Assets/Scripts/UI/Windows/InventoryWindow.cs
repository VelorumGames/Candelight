using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Window
{
    public class InventoryWindow : AUIWindow
    {
        [SerializeField] RectTransform _unactiveContainer;
        [SerializeField] RectTransform _activeContainer;

        Inventory _inv;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
        }

        protected override void OnStart()
        {
            _inv.LoadItems();
        }

        protected override void OnClose()
        {
            _inv.UnloadItems();
        }

        public void ManageItemPosition(GameObject item, Vector3 pos, bool isOff)
        {
            item.GetComponent<Image>().SetNativeSize();
            item.GetComponent<RectTransform>().SetParent(isOff ? _unactiveContainer : _activeContainer);
            item.GetComponent<RectTransform>().localPosition = pos;
        }
    }
}
