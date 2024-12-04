using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class UIFadeOnDisable : MonoBehaviour
    {
        [SerializeField] float _endFade;
        [SerializeField] float _duration;
        Image _img;
        TextMeshProUGUI _text;

        private void Awake()
        {
            _img = GetComponent<Image>();
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void FadeElement()
        {
            if (_img)
            {
                _img.DOFade(_endFade, _duration).SetUpdate(true).Play();
            }
            else if (_text)
            {
                _text.DOFade(_endFade, _duration).SetUpdate(true).Play();
            }
        }
    }
}