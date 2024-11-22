using DG.Tweening;
using Items;
using Items.ConcreteItems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemNotification : MonoBehaviour
    {
        [SerializeField] float _startDuration;
        [SerializeField] float _duration;

        [SerializeField] TextMeshProUGUI _itemName;
        [SerializeField] TextMeshProUGUI _itemsubDesc;
        [SerializeField] Image _itemSprite;

        private void OnEnable()
        {
            GetComponent<RectTransform>().DOLocalMove(new Vector3(-300f, 0f, 0f), _startDuration).OnComplete(() => Invoke("ResetNotification", _duration)).Play().SetUpdate(true);
        }

        public void LoadItemInfo(ItemInfo data)
        {
            _itemName.text = data.Name;
            _itemSprite.sprite = data.ItemSprite;
            _itemsubDesc.text = data.SubDescription;
        }

        void ResetNotification()
        {
            GetComponent<RectTransform>().DOLocalMove(new Vector3(-1000f, 0f, 0f), _startDuration).OnComplete(() => gameObject.SetActive(false)).Play().SetUpdate(true);
        }
    }
}