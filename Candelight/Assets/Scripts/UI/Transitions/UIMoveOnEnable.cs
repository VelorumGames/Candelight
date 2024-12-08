using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIMoveOnEnable : MonoBehaviour
    {
        [SerializeField] Vector3 _endPos;
        [SerializeField] float _duration;
        [SerializeField] Ease _easeType;

        Vector3 _oPos;
        RectTransform _rect;

        UISoundManager _sound;

        private void Awake()
        {
            _sound = FindObjectOfType<UISoundManager>();

            _rect = GetComponent<RectTransform>();
            _oPos = _rect.localPosition;
        }

        private void OnEnable()
        {
            _sound?.PlayMove();

            _rect.localPosition = _oPos;
            _rect.DOLocalMove(_endPos, _duration).SetUpdate(true).SetEase(_easeType).Play();
        }
    }
}