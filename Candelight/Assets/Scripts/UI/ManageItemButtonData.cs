using Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ManageItemButtonData : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _cat;
        [SerializeField] TextMeshProUGUI _frags;
        [SerializeField] Color[] _colors;

        ShowCurrentItemInfo _show;

        private void Awake()
        {
            _show = FindObjectOfType<ShowCurrentItemInfo>();
        }

        private void Start()
        {
            LoadData(GetComponent<AItem>());
        }

        void LoadData(AItem item)
        {
            _icon.sprite = item.Data.ItemSprite;
            _name.text = item.Data.Name;

            switch(item.Data.Category)
            {
                case EItemCategory.Common:
                    _cat.color = _colors[0];
                    _cat.text = "Común";
                    break;
                case EItemCategory.Rare:
                    _cat.color = _colors[1];
                    _cat.text = "Raro";
                    break;
                case EItemCategory.Epic:
                    _cat.color = _colors[2];
                    _cat.text = "Épico";
                    break;
                case EItemCategory.Legendary:
                    _cat.color = _colors[3];
                    _cat.text = "Legendario";
                    break;
            }

            _frags.text = $"{(int)item.Data.Category}";
        }

        public void SendInfo()
        {
            if (_show) _show.ShowInfo(GetComponent<AItem>().Data);
        }
    }
}