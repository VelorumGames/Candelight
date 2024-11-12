using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InstructionUI : MonoBehaviour
    {
        RectTransform _trans;
        Vector3 _oPos;

        [SerializeField] float _animTime;
        [SerializeField] float _initialScale;
        [SerializeField] float _initialPosRange;
        [SerializeField] float _initialRotRange;

        private void Awake()
        {
            _trans = GetComponent<RectTransform>();
            _oPos = _trans.localPosition;
        }


        private void OnEnable()
        {
            _trans.localScale = _initialScale * Vector3.one;
            _trans.localPosition = _oPos + new Vector3(Random.Range(-_initialPosRange, _initialPosRange), Random.Range(-_initialPosRange, _initialPosRange), 0f);
            _trans.localRotation = Quaternion.Euler(Random.Range(-_initialRotRange, _initialRotRange), Random.Range(-_initialRotRange, _initialRotRange), Random.Range(-_initialRotRange, _initialRotRange));

            _trans.DOScale(0.5f * Vector3.one, _animTime).Play();
            _trans.DOLocalMove(_oPos, _animTime).Play();
            _trans.DOLocalRotate(new Vector3(0f, 0f, 0f), _animTime).Play();
        }
    }
}
