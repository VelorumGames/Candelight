using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Window
{
    public abstract class AUIWindow : MonoBehaviour
    {
        private void OnEnable()
        {
            OnStart();
        }

        protected abstract void OnStart();
        protected abstract void OnClose();

        private void OnDisable()
        {
            OnClose();
        }
    }
}
