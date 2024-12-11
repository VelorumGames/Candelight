using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    public class MobileInteract : MonoBehaviour
    {
        Action _inter;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(Action act)
        {
            _inter = act;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _inter = null;
            gameObject.SetActive(false);
        }

        public void Interact() => _inter?.Invoke();
    }
}