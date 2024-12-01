using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Visual;
using TMPro;

namespace UI.Window
{
    public class OptionsWindow : AUIWindow
    {
        ApplyOptions _options;

        [SerializeField] Color _selectedColor;
        [SerializeField] TextMeshProUGUI _videoText;
        [SerializeField] TextMeshProUGUI _audioText;
        [SerializeField] TextMeshProUGUI _accesText;
        [Space(10)]
        [SerializeField] Slider _brightness;
        [SerializeField] Slider _contrast;
        [SerializeField] Slider _saturation;
        [SerializeField] Slider _genVolume;
        [SerializeField] Slider _soundVolume;
        [SerializeField] Slider _musicVolume;
        [SerializeField] Toggle _autoSave;

        private void Awake()
        {
            _options = FindObjectOfType<ApplyOptions>();

            _brightness.value = GameSettings.Brightness;
            _contrast.value = GameSettings.Contrast;
            _saturation.value = GameSettings.Saturation;
            _genVolume.value = GameSettings.GeneralVolume;
            _soundVolume.value = GameSettings.SoundVolume;
            _musicVolume.value = GameSettings.MusicVolume;

            _autoSave.isOn = GameSettings.AutoSave;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnClose()
        {

        }

        public void ChangeColor(int id)
        {
            switch(id)
            {
                case 0:
                    _videoText.color = _selectedColor;
                    _audioText.color = Color.white;
                    _accesText.color = Color.white;
                    break;
                case 1:
                    _videoText.color = Color.white;
                    _audioText.color = _selectedColor;
                    _accesText.color = Color.white;
                    break;
                case 2:
                    _videoText.color = Color.white;
                    _audioText.color = Color.white;
                    _accesText.color = _selectedColor;
                    break;
            }
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

        public void LoadAutoSave(bool value)
        {
            GameSettings.AutoSave = value;
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