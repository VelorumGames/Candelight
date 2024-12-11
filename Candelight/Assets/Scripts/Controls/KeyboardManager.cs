using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Controls
{
    public class KeyboardManager : MonoBehaviour
    {
        TMP_InputField _currentText;

        bool _shift;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(TMP_InputField textToEdit)
        {
            _currentText = textToEdit;
            gameObject.SetActive(true);
        }

        public void AddCharacter(string s)
        {
            _currentText.text += _shift ? s.ToUpper() : s.ToLower();
        }

        public void RemoveCharacter()
        {
            if (_currentText.text.Length > 0) _currentText.text = _currentText.text.Substring(0, _currentText.text.Length - 1);
        }

        public void ShiftMode()
        {
            _shift = !_shift;
        }
    }
}
