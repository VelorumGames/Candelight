using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Controls
{
    public class KeyboardKey : MonoBehaviour
    {
        KeyboardManager _keyboard;
        string c;

        private void Awake()
        {
            _keyboard = GetComponentInParent<KeyboardManager>();
            c = GetComponentInChildren<TextMeshProUGUI>().text;
        }

        public void AddCharacter() => _keyboard?.AddCharacter(c);
        public void TurnShift() => _keyboard?.ShiftMode();
        public void RemoveCharacter() => _keyboard?.RemoveCharacter();
    }
}