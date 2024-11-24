using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] RectTransform _bottomCandle;
        [SerializeField] float _oPos;
        [SerializeField] float _finalPos;
        [Space(10)]
        [SerializeField] Color[] _upgradeColors;

        [SerializeField] TextMeshProUGUI _damageNotif;

        float _prevCandle = 0;

        private void OnEnable()
        {
            _world.OnCandleChanged += UpdateCandle;
            _prevCandle = _world.Candle;
        }

        private void Start()
        {
            ManageCandleAppearance(Upgrades.StartElement);
            UpdateCandle(_world.Candle);
        }

        void UpdateCandle(float candle)
        {
            float rem = candle / _world.MAX_CANDLE;
            _topCandle.localPosition = new Vector3(_topCandle.localPosition.x, Mathf.Lerp(_finalPos, _oPos, rem), _topCandle.localPosition.z);

            if (candle < _prevCandle)
            {
                _damageNotif.text = $"- {_prevCandle - candle} HP";
                _damageNotif.gameObject.SetActive(true);
                Invoke("ResetDamageNotif", 1f);
            }

            if (rem >= 0.8f) _topCandle.GetComponent<Image>().sprite = _candleSprites[0];
            else if (rem >= 0.6f) _topCandle.GetComponent<Image>().sprite = _candleSprites[1];
            else if (rem >= 0.4f) _topCandle.GetComponent<Image>().sprite = _candleSprites[2];
            else if (rem >= 0.2f) _topCandle.GetComponent<Image>().sprite = _candleSprites[3];
            else if (rem > 0f) _topCandle.GetComponent<Image>().sprite = _candleSprites[4];
            else _topCandle.GetComponent<Image>().sprite = _candleSprites[5];

            _prevCandle = candle;
        }

        public void ResetDamageNotif()
        {
            _damageNotif.gameObject.SetActive(false);
        }

        void ManageCandleAppearance(EStartingElement el)
        {
            switch(el)
            {
                case EStartingElement.Fire:
                    break;
                case EStartingElement.Electric:
                    _topCandle.GetComponent<Image>().color = _upgradeColors[0];
                    _bottomCandle.GetComponent<Image>().color = _upgradeColors[0];
                    break;
                case EStartingElement.Cosmic:
                    _topCandle.GetComponent<Image>().color = _upgradeColors[1];
                    _bottomCandle.GetComponent<Image>().color = _upgradeColors[1];
                    break;
                case EStartingElement.Phantom:
                    _topCandle.GetComponent<Image>().color = _upgradeColors[2];
                    _bottomCandle.GetComponent<Image>().color = _upgradeColors[2];
                    break;
            }
        }

        private void OnDisable()
        {
            _world.OnCandleChanged -= UpdateCandle;
        }
    }
}