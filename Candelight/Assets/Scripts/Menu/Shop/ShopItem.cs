using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Menu.Shop
{
    public class ShopItem : MonoBehaviour
    {
        public int Id;

        ShopManager _shop;
        public string Name;
        public string Desc;
        public EStartingElement Element;
        public float Cost;

        public GameObject Selector;

        bool _bought;
        [SerializeField] TextMeshProUGUI _status;
        [SerializeField] TextMeshProUGUI _desc;

        bool _active;

        ShopItem[] _items;

        private void Awake()
        {
            _shop = FindObjectOfType<ShopManager>();
            _items = FindObjectsOfType<ShopItem>(true);
        }

        private void Start()
        {
            _status.text = "";
        }

        public void LoadInfo()
        {
            _shop.ShowCurrentInfo(this);
        }

        public void Activate()
        {
            if (_active && _bought) Deactivate();
            else if (_bought)
            {
                foreach (var item in _items)
                {
                    if (item != this) item.Deactivate();
                }

                _active = true;

                Debug.Log("Se registra como elemento: " + Element);
                Upgrades.StartElement = Element;
                _desc.text = "MEJORA ACTIVADA";
                Selector.SetActive(true);
            }
        }

        public void Deactivate()
        {
            if (_bought)
            {
                _active = false;

                Debug.Log("Se vuelve a Fire");
                Upgrades.StartElement = EStartingElement.Fire;
                _desc.text = "MEJORA DESACTIVADA";
                Selector.SetActive(false);
            }
        }

        public bool IsBought() => _bought;
        public void SetBought(bool b)
        {
            _bought = b;
            _status.text = _bought ? "Comprado" : "";
        }
    }
}