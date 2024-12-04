using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class UIMoveOnDisable : MonoBehaviour
    {
        [SerializeField] Vector3 _endPos;
        [SerializeField] float _duration;
        [SerializeField] Ease _easeType;

        RectTransform _rect;

        UISoundManager _sound;

        private void Awake()
        {
            _sound = FindObjectOfType<UISoundManager>();

            _rect = GetComponent<RectTransform>();
        }

        public void DisableElement()
        {
            _sound.PlayMove();

            _rect.DOLocalMove(_endPos, _duration).SetUpdate(true).SetEase(_easeType).Play().OnComplete(() => gameObject.SetActive(false));
        }
    }
}