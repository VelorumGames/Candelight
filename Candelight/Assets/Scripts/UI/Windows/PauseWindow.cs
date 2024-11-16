using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Window
{
    public class PauseWindow : AUIWindow
    {
        protected override void OnStart()
        {
            Time.timeScale = 0f;
        }

        protected override void OnClose()
        {
            Time.timeScale = 1f;
        }
    }
}
