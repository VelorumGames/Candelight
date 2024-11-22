using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum EGameState
    {
        None,
        Loading,
        Saving,
        Database
    }
    public class UIGameState : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] Image _img;
        [SerializeField] Sprite[] _sprites;
        [SerializeField] string[] _texts;

        private void Awake()
        {
            Hide();
        }

        public void Show(EGameState state)
        {
            _img.color = Color.white;
            _img.sprite = _sprites[(int)state - 1];
            _text.text = _texts[(int)state - 1];
            _img.SetNativeSize();
        }

        public void Hide()
        {
            _img.color = new Color(1f, 1f, 1f, 0f);
            _text.text = "";
        }
    }
}