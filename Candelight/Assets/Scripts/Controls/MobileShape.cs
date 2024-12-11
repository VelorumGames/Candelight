using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class MobileShape : MonoBehaviour
    {
        InputManager _input;

        private void Awake()
        {
            _input = FindObjectOfType<InputManager>();
        }

        public void StartShape()
        {
            if (Application.isMobilePlatform)
            {
                _input.StartSpellMode(new InputAction.CallbackContext());
            }
        }

        public void ExitShape()
        {
            if (Application.isMobilePlatform)
            {
                _input.StopSpellMode(new InputAction.CallbackContext());
            }
        }
    }
}