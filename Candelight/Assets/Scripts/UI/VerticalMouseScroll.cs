using Controls;
using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class VerticalMouseScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        RectTransform _content;
        InputManager _input;
        Inventory _inv;

        [SerializeField] float _scrollSens;
        [SerializeField] bool _activeItems;

        Action _updateAction;

        private void Awake()
        {
            _content = GetComponent<ScrollRect>().content;
            _input = FindObjectOfType<InputManager>();
            _inv = FindObjectOfType<Inventory>();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _updateAction = RegisterScroll;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _updateAction = null;
        }

        private void Update()
        {
            if (_updateAction != null) _updateAction();

            if (!_activeItems) Debug.Log(_content.localPosition);
            _content.localPosition = new Vector3(_content.localPosition.x, Mathf.Clamp(_content.localPosition.y, _activeItems ? -_inv.ActiveItems.Count * 40f : -_inv.UnactiveItems.Count * 40f, 0f), _content.localPosition.z);
        }

        void RegisterScroll()
        {
            _content.localPosition += _input.GetScrollData() * _scrollSens * Vector3.up;
        }
    }
}