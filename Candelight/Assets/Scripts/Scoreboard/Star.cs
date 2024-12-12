using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace Scoreboard
{
    public class Star : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        WorldInfo _world;

        ScoreData _data;
        [SerializeField] GameObject _starInfo;
        [SerializeField] TextMeshPro _name;
        [SerializeField] TextMeshPro _score;
        [SerializeField] SpriteRenderer _sprite;
        [SerializeField] Sprite[] _startSprites;

        static List<GameObject> _currentInfos = new List<GameObject>();

        float _oScale;

        //bool _shown;

        public void LoadData(ScoreData d)
        {
            _data = d;
            _name.text = _data.Name;
            _score.text = _data.Score != 1 ? $"{_data.Score} zonas iluminadas" : $"{_data.Score} zona iluminada";
            //_sprite.transform.localScale = (_data.Score / 20f) * Vector3.one;

            if (_data.Score <= 0) _sprite.color = new Color(1f, 1f, 1f, 0f);
            else if (_data.Score < 5) _sprite.sprite = _startSprites[0];
            else if (_data.Score < 15) _sprite.sprite = _startSprites[1];
            else if (_data.Score < 30) _sprite.sprite = _startSprites[2];
            else if (_data.Score < 50) _sprite.sprite = _startSprites[3];
            else if (_data.Score < _world.MAX_NODES - 5) _sprite.sprite = _startSprites[4];
            else _sprite.sprite = _startSprites[5];

            if (_data.Score > 0) _sprite.color = new Color(1f, 1f, 1f, 1f);

            _oScale = transform.localScale.x;
        }
        private void OnMouseDown()
        {
            if (_currentInfos.Contains(_starInfo))
            {
                _starInfo.SetActive(false);
                _currentInfos.Remove(_starInfo);
            }
            else
            {
                _starInfo.SetActive(true);
                _currentInfos.Add(_starInfo);

                foreach (var info in _currentInfos)
                {
                    if (info != _starInfo && Vector3.Distance(info.transform.position, _starInfo.transform.position) < 3f)
                    {
                        info.SetActive(false);
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(_oScale * 1.1f, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_oScale / 1.1f, 0.5f);
        }
    }
}
