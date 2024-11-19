using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ConnectBook : MonoBehaviour
    {
        PlayerController _player;
        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        public void OpenBook()
        {
            if (_player != null) _player.OnBook(new InputAction.CallbackContext());
        }
    }
}