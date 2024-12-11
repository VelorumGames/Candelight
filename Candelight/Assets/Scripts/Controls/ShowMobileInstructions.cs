using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controls
{
    public class ShowMobileInstructions : MonoBehaviour
    {
        InputManager _input;

        private void Awake()
        {
            _input = FindObjectOfType<InputManager>();

            _input.OnStartElementMode += Activate;
            _input.OnStartShapeMode += Activate;

            _input.OnExitElementMode += Deactivate;
            _input.OnExitShapeMode += Deactivate;

            gameObject.SetActive(false);
        }

        void Activate() => gameObject.SetActive(true);
        void Deactivate() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            _input.OnStartElementMode -= Activate;
            _input.OnStartShapeMode -= Activate;

            _input.OnExitElementMode -= Deactivate;
            _input.OnExitShapeMode -= Deactivate;
        }
    }
}