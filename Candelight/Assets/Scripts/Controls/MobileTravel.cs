using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Controls
{
    public class MobileTravel : MonoBehaviour
    {
        PlayerController _cont;
        InputAction.CallbackContext _ctx = new InputAction.CallbackContext();

        private void Awake()
        {
            _cont = FindObjectOfType<PlayerController>();
        }

        public void Travel()
        {
            if (SceneManager.GetActiveScene().name == "WorldScene") _cont.OnConfirmPath(_ctx);
        }
    }
}