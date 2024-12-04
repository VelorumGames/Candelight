using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controls;

namespace UI.Window
{
    public class PauseWindow : AUIWindow
    {
        float _prev;

        UISoundManager _sound;

        private void Awake()
        {
            _sound = FindObjectOfType<UISoundManager>();
        }

        protected override void OnStart()
        {
            _prev = Time.timeScale;
            Time.timeScale = 0f;

            _sound.PlayOpenPause();
        }

        protected override void OnClose()
        {
            Time.timeScale = _prev;

            _sound.PlayClosePause();
        }
    }
}
