using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controls;

namespace UI.Window
{
    public class PauseWindow : AUIWindow
    {
        float _prev;

        protected override void OnStart()
        {
            _prev = Time.timeScale;
            Time.timeScale = 0f;
        }

        protected override void OnClose()
        {
            Time.timeScale = _prev;
        }
    }
}
