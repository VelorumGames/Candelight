using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ManageUIMode : MonoBehaviour
    {
        public string Id;

        Image _img;
        TextMeshProUGUI _text;

        UIManager _ui;

        private void Awake()
        {
            _img = GetComponent<Image>();
            _text = GetComponent<TextMeshProUGUI>();

            _ui = FindObjectOfType<UIManager>();
            if (_ui) _ui.UiModeElements.Add(this);
        }

        public void Show()
        {
            if (_img) _img.DOFade(1f, 0.2f).SetUpdate(true).Play(); 
            else if (_text) _text.DOFade(1f, 0.2f).SetUpdate(true).Play();
        }

        public void Hide()
        {
            if (_img) _img.DOFade(0f, 0.2f).SetUpdate(true).Play();
            else if (_text) _text.DOFade(0f, 0.2f).SetUpdate(true).Play();
        }
    }
}