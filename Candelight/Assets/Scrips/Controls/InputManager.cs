using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;

namespace Controls
{
    public enum EControlMap
    {
        Level
    }

    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public InputActionAsset Input;
        [SerializeField] EControlMap _initialControls;
        PlayerController _cont;

        InputActionMap _currentMap;
        InputActionMap _levelMap;

        InputAction _move;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _cont = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            InitializeControls();
            LoadControls(_initialControls);
        }

        void InitializeControls()
        {
            _levelMap = Input.FindActionMap("Level");

            _move = _levelMap.FindAction("Move");

        }

        public void LoadControls(EControlMap map)
        {
            if (_currentMap != null) _currentMap.Disable();
            switch(map)
            {
                case EControlMap.Level:
                    _currentMap = _levelMap;
                    break;
            }
            _currentMap.Enable();
        }

        private void FixedUpdate()
        {
            if (_move.IsPressed()) _cont.OnMove(_move.ReadValue<Vector2>());
        }
    }
}