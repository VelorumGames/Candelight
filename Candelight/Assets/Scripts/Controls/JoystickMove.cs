using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controls
{
    public class JoystickMove : MonoBehaviour
    {
        public Joystick MovementJoystick;
        PlayerController _cont;
        bool _world;

        InputManager _input;

        private void Awake()
        {
            _cont = FindObjectOfType<PlayerController>();
            _input = FindObjectOfType<InputManager>();

            _world = SceneManager.GetActiveScene().name == "WorldScene";

            _input.OnStartElementMode += Deactivate;
            _input.OnStartShapeMode += Deactivate;

            _input.OnExitElementMode += Activate;
            _input.OnExitShapeMode += Activate;
        }

        private void FixedUpdate()
        {
            if (MovementJoystick.Direction.x != 0 && MovementJoystick.Direction.y != 0)
            {
                if (!_world) _cont.OnMove(MovementJoystick.Direction);
                else _cont.OnChoosePath(MovementJoystick.Direction);
            }
        }

        void Activate() => gameObject.SetActive(true);
        void Deactivate() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            _input.OnStartElementMode -= Deactivate;
            _input.OnStartShapeMode -= Deactivate;

            _input.OnExitElementMode -= Activate;
            _input.OnExitShapeMode -= Activate;
        }
    }
}
