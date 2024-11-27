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

        private void Awake()
        {
            _shop = FindObjectOfType<ShopManager>();
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
            if (_bought)
            {
                _active = !_active;

                Upgrades.StartElement = _active ? EStartingElement.Fire : Element;
                _desc.text = _active ? "MEJORA ACTIVADA" : "MEJORA DESACTIVADA";
                Selector.SetActive(_active);
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