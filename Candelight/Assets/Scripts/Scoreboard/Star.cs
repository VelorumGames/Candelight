using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using World;

namespace Scoreboard
{
    public class Star : MonoBehaviour
    {
        WorldInfo _world;

        UserData _data;
        [SerializeField] GameObject _starInfo;
        [SerializeField] TextMeshPro _name;
        [SerializeField] TextMeshPro _score;
        [SerializeField] SpriteRenderer _sprite;
        [SerializeField] Sprite[] _startSprites;

        bool _shown;

        public void LoadData(UserData d)
        {
            _data = d;
            _name.text = _data.Name;
            _score.text = $"{_data.Score} zonas iluminadas";
            //_sprite.transform.localScale = (_data.Score / 20f) * Vector3.one;

            if (_data.Score <= 0) _sprite.color = new Color(0f, 0f, 0f, 0f);
            else if (_data.Score < 5) _sprite.sprite = _startSprites[0];
            else if (_data.Score < 15) _sprite.sprite = _startSprites[1];
            else if (_data.Score < 30) _sprite.sprite = _startSprites[2];
            else if (_data.Score < 50) _sprite.sprite = _startSprites[3];
            else if (_data.Score < _world.MAX_NODES - 5) _sprite.sprite = _startSprites[4];
            else _sprite.sprite = _startSprites[5];
        }
        private void OnMouseDown()
        {
            if (_shown)
            {
                _shown = false;
                _starInfo.SetActive(false);
            }
            else
            {
                _shown = true;
                _starInfo.SetActive(true);
            }
        }

    }
}
