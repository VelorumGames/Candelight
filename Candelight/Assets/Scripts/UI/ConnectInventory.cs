using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ConnectInventory : MonoBehaviour
    {
        PlayerController _player;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        public void OpenInventory()
        {
            if (_player != null) _player.OnInventory(new InputAction.CallbackContext());
        }
    }
}