using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class MobileInstruction : MonoBehaviour
    {
        string s;
        InputManager _input;
        InputAction.CallbackContext _ctx = new InputAction.CallbackContext();

        private void Awake()
        {
            s = GetComponentInChildren<TextMeshProUGUI>().text;
            _input = FindObjectOfType<InputManager>();
        }

        public void ClickInstr()
        {
            switch(s)
            {
                case "w":
                    _input.RegisterSpellUp(_ctx);
                    break;
                case "s":
                    _input.RegisterSpellDown(_ctx);
                    break;
                case "d":
                    _input.RegisterSpellRight(_ctx);
                    break;
                case "a":
                    _input.RegisterSpellLeft(_ctx);
                    break;
            }
        }
    }
}
