using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scoreboard
{
    public class Star : MonoBehaviour
    {
        UserData _data;
        [SerializeField] TextMeshPro _text;
        [SerializeField] SpriteRenderer _sprite;

        bool _shown;

        public void LoadData(UserData d)
        {
            _data = d;
            _text.text = $"{_data.Name}\nScore: {_data.Score}";
            _sprite.transform.localScale = (_data.Score / 20f) * Vector3.one;
        }
        private void OnMouseDown()
        {
            if (_shown)
            {
                _shown = false;
                _text.gameObject.SetActive(false);
            }
            else
            {
                _shown = true;
                _text.gameObject.SetActive(true);
            }
        }

    }
}
