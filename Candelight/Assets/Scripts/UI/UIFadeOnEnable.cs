using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFadeOnEnable : MonoBehaviour
    {
        [SerializeField] float _endFade;
        [SerializeField] float _duration;

        float _oFade;

        public bool FadeOut;

        Image _img;
        TextMeshProUGUI _text;

        private void Awake()
        {
            _img = GetComponent<Image>();
            _text= GetComponent<TextMeshProUGUI>();

            if (_img) _oFade = _img.color.a;
            else if (_text) _oFade = _text.color.a;
        }

        private void OnEnable()
        {
            if (_img)
            {
                if (!FadeOut) _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, 0f);
                else _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, _oFade);
                _img.DOFade(_endFade, _duration).SetUpdate(true).Play();
            }
            else if (_text)
            {
                if (!FadeOut) _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
                else _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _oFade);
                _text.DOFade(_endFade, _duration).SetUpdate(true).Play();
            }
        }
    }
}
