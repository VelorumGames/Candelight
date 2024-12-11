using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class OpenKeyboard : MonoBehaviour, IPointerDownHandler
    {
        KeyboardManager _keyboard;

        private void Awake()
        {
            _keyboard = FindObjectOfType<KeyboardManager>(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Application.isMobilePlatform && _keyboard) _keyboard.Show(GetComponent<TMP_InputField>());
        }
    }
}