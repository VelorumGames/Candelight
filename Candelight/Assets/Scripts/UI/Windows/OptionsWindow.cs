using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Visual;

namespace UI.Window
{
    public class OptionsWindow : AUIWindow
    {
        ApplyOptions _options;

        [SerializeField] Slider _brightness;
        [SerializeField] Slider _contrast;
        [SerializeField] Slider _saturation;
        [SerializeField] Slider _genVolume;
        [SerializeField] Slider _soundVolume;
        [SerializeField] Slider _musicVolume;

        private void Awake()
        {
            _options = FindObjectOfType<ApplyOptions>();

            _brightness.value = GameSettings.Brightness;
            _contrast.value = GameSettings.Contrast;
            _saturation.value = GameSettings.Saturation;
            _genVolume.value = GameSettings.GeneralVolume;
            _soundVolume.value = GameSettings.SoundVolume;
            _musicVolume.value = GameSettings.MusicVolume;
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

        public void ResetSettings()
        {
            if (_options != null)
            {
                _options.ResetSettings();
            }

            GameSettings.Brightness = 0;
            GameSettings.Contrast = 0;
            GameSettings.Saturation = 0;

            GameSettings.GeneralVolume = 0;
            GameSettings.SoundVolume = 0;
            GameSettings.MusicVolume = 0;

            _brightness.value = GameSettings.Brightness;
            _contrast.value = GameSettings.Contrast;
            _saturation.value = GameSettings.Saturation;
            _genVolume.value = GameSettings.GeneralVolume;
            _soundVolume.value = GameSettings.SoundVolume;
            _musicVolume.value = GameSettings.MusicVolume;
        }

    }
}