using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;

namespace Controls
{
    public enum EControlMap
    {
        Level,
        World
    }

    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public InputActionAsset Input;
        [SerializeField] EControlMap _initialControls;
        [SerializeField] bool _startControls;
        PlayerController _cont;

        InputActionMap _currentMap;
        InputActionMap _worldMap;
        InputActionMap _levelMap;

        InputAction _move;
        InputAction _choosePath;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            InitializeControls();
        }

        private void Start()
        {
            LoadControls(_initialControls);
        }

        void InitializeControls()
        {
            Debug.Log("Se inicializan los controles");
            _cont = FindObjectOfType<PlayerController>();

            //Level
            _levelMap = Input.FindActionMap("Level");

            _move = _levelMap.FindAction("Move");
            InputAction interact = _levelMap.FindAction("Interact");
            interact.performed += _cont.OnInteract;

            //World
            _worldMap = Input.FindActionMap("World");

            _choosePath = _worldMap.FindAction("ChoosePath");
            InputAction confirmPath = _worldMap.FindAction("ConfirmPath");
            confirmPath.performed += _cont.OnConfirmPath;
            InputAction worldInteract = _worldMap.FindAction("Interact");
            worldInteract.performed += _cont.OnInteract;
        }

        public void LoadControls(EControlMap map)
        {
            Debug.Log("Mapa previo: " + _currentMap);
            if (_currentMap != null) _currentMap.Disable();
            switch(map)
            {
                case EControlMap.Level:
                    _currentMap = _levelMap;
                    break;
                case EControlMap.World:
                    _currentMap = _worldMap;
                    break;
            }
            Debug.Log("Mapa colocado: " + _currentMap);
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            InputAction interact = _levelMap.FindAction("Interact");
            interact.performed -= _cont.OnInteract;
            _move.Disable();
            
            InputAction confirmPath = _worldMap.FindAction("ConfirmPath");
            confirmPath.performed -= _cont.OnConfirmPath;
            InputAction worldInteract = _worldMap.FindAction("Interact");
            worldInteract.performed -= _cont.OnInteract;
            _choosePath.Disable();
        }

        private void FixedUpdate()
        {
            if (_move.IsPressed()) _cont.OnMove(_move.ReadValue<Vector2>());
            if (_choosePath.IsPressed()) _cont.OnChoosePath(_choosePath.ReadValue<Vector2>());
        }
    }
}