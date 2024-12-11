using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu
{
    public class PauseButton : MonoBehaviour
    {
        PlayerController _player;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        public void SearchForPause()
        {
            _player?.OnPause(new InputAction.CallbackContext());
        }
    }
}