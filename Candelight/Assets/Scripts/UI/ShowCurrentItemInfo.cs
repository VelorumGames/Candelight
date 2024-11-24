using Items.ConcreteItems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowCurrentItemInfo : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _descr;

        private void Awake()
        {
            _icon.gameObject.SetActive(false);
            _name.gameObject.SetActive(false);
            _descr.gameObject.SetActive(false);
        }

        public void ShowInfo(ItemInfo data)
        {
            _icon.gameObject.SetActive(true);
            _name.gameObject.SetActive(true);
            _descr.gameObject.SetActive(true);

            _icon.sprite = data.ItemSprite;
            _name.text = data.Name;
            _descr.text = data.Description;
        }
    }
}