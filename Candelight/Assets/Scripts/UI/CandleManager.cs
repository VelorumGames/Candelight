using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI
{
    public class CandleManager : MonoBehaviour
    {
        [SerializeField] Sprite[] _candleSprites;
        [SerializeField] WorldInfo _world;
        [SerializeField] RectTransform _topCandle;
        [SerializeField] float _oPos;
        [SerializeField] float _finalPos;

        private void OnEnable()
        {
            _world.OnCandleChanged += UpdateCandle;
        }

        private void Start()
        {
            UpdateCandle(_world.Candle);
        }

        void UpdateCandle(float candle)
        {
            float rem = candle / _world.MAX_CANDLE;
            _topCandle.localPosition = new Vector3(_topCandle.localPosition.x, Mathf.Lerp(_finalPos, _oPos, rem), _topCandle.localPosition.z);

            if (rem >= 0.8f) _topCandle.GetComponent<Image>().sprite = _candleSprites[0];
            else if (rem >= 0.6f) _topCandle.GetComponent<Image>().sprite = _candleSprites[1];
            else if (rem >= 0.4f) _topCandle.GetComponent<Image>().sprite = _candleSprites[2];
            else if (rem >= 0.2f) _topCandle.GetComponent<Image>().sprite = _candleSprites[3];
            else if (rem > 0f) _topCandle.GetComponent<Image>().sprite = _candleSprites[4];
            else _topCandle.GetComponent<Image>().sprite = _candleSprites[5];
        }

        private void OnDisable()
        {
            _world.OnCandleChanged -= UpdateCandle;
        }
    }
}