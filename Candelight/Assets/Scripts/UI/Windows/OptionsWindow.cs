using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visual;

namespace UI.Window
{
    public class OptionsWindow : AUIWindow
    {
        ApplyOptions _options;

        private void Awake()
        {
            _options = FindObjectOfType<ApplyOptions>();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnClose()
        {

        }

        public void LoadBrightness(float value)
        {
            GameSettings.Brightness = value;
            if (_options != null) _options.ApplySettings();
        }
        public void LoadContrast(float value)
        {
            GameSettings.Contrast = value;
            if (_options != null) _options.ApplySettings();
        }
        public void LoadSaturation(float value)
        {
            GameSettings.Saturation = value;
            if (_options != null) _options.ApplySettings();
        }

        public void LoadGenVolume(float value)
        {
            GameSettings.GeneralVolume = value;
            if (_options != null) _options.ApplySettings();
        }
        public void LoadSoundVolume(float value)
        {
            GameSettings.SoundVolume = value;
            if (_options != null) _options.ApplySettings();
        }
        public void LoadMusicVolume(float value)
        {
            GameSettings.MusicVolume = value;
            if (_options != null) _options.ApplySettings();
        }

    }
}