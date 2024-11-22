using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        Tween _scale;
        Tween _move;
        Tween _rotate;

        Color _oColor;

        private void Awake()
        {
            _trans = GetComponent<RectTransform>();
            _oPos = _trans.localPosition;

            float oScale = _trans.localScale.x;

            _scale = _trans.DOScale(oScale, _animTime).SetAutoKill(false).OnPlay(() => GetComponent<Image>().SetNativeSize());
            _move = _trans.DOLocalMove(_oPos, _animTime).SetAutoKill(false);
            _rotate = _trans.DOLocalRotate(new Vector3(0f, 0f, 0f), _animTime).SetAutoKill(false);

            _oColor = GetComponent<Image>().color;
        }


        private void OnEnable()
        {
            _trans.localScale = _initialScale * Vector3.one;
            _trans.localPosition = _oPos + new Vector3(Random.Range(-_initialPosRange, _initialPosRange), Random.Range(-_initialPosRange, _initialPosRange), 0f);
            _trans.localRotation = Quaternion.Euler(Random.Range(-_initialRotRange, _initialRotRange), Random.Range(-_initialRotRange, _initialRotRange), Random.Range(-_initialRotRange, _initialRotRange));

            _scale.Restart();
            _scale.Pause();
            _scale.Play();
            _move.Restart();
            _move.Play();
            _rotate.Restart();
           _rotate.Play();
        }

        public Color GetColor() => _oColor;
    }
}
