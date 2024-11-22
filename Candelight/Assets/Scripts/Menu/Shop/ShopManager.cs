using Menu.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Menu.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] ShopItem[] _shopItems;

        [SerializeField] TextMeshProUGUI _desc;
        [SerializeField] TextMeshProUGUI _cost;

        ShopItem _currentItem;

        List<int> _boughtIds = new List<int>();

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            StartCoroutine(CheckForBoughtItems());
        }

        public void ShowCurrentInfo(ShopItem item)
        {
            _currentItem = item;
            _desc.text = $"{item.Name}: {item.Desc}";
            _cost.text = $"Coste: {item.Cost}€";
        }

        public void Buy()
        {
            if (_currentItem != null)
            {
                if (_currentItem.IsBought())
                {
                    _desc.text = "¡Ya has comprado esa mejora!";
                    _cost.text = "";
                }
                else FindObjectOfType<UIManager>().ShowWarning(() => StartCoroutine(ManageBuy()), $"Pagarás {_currentItem.Cost} por {_currentItem.Name}.\n¿Estás seguro?");
            }
        }

        IEnumerator ManageBuy()
        {
            _currentItem.SetBought(true);
            _boughtIds.Add(_currentItem.Id);

            ShopData data = new ShopData(this);

            _ui.ShowState(EGameState.Database);
            yield return Database.Send($"Shop/{SaveSystem.PlayerName}", data);
            _ui.HideState();
        }

        IEnumerator CheckForBoughtItems()
        {
            _ui.ShowState(EGameState.Database);
            yield return Database.Get<ShopData>($"Shop/{SaveSystem.PlayerName}", RecieveData);
            _ui.HideState();
        }

        void RecieveData(ShopData data)
        {
            if (data != null)
            {
                foreach(var id in data.Ids)
                {
                    _shopItems[id].SetBought(true);
                }
            }
        }

        public int[] GetIds() => _boughtIds.ToArray();
    }

    [Serializable]
    public class ShopData
    {
        public int[] Ids;

        public ShopData(ShopManager shop)
        {
            Ids = shop.GetIds();
        }
    }
}

