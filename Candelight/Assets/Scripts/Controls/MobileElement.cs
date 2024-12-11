using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class MobileElement : MonoBehaviour
    {
        InputManager _input;

        private void Awake()
        {
            _input = FindObjectOfType<InputManager>();
        }

        public void StartElement()
        {
            if (Application.isMobilePlatform)
            {
                _input.StartElementMode(new InputAction.CallbackContext());
            }
        }

        public void ExitElement()
        {
            if (Application.isMobilePlatform)
            {
                _input.StopElementMode(new InputAction.CallbackContext());
            }
        }
    }
}