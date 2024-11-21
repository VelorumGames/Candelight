using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFadeOnEnable : MonoBehaviour
    {
        [SerializeField] float _endFade;
        [SerializeField] float _duration;

        Image _img;

        private void Awake()
        {
            _img = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, 0f);
            _img.DOFade(_endFade, _duration).SetUpdate(true).Play();
        }
    }
}
