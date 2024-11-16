using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Visual
{
    public class ApplyOptions : MonoBehaviour
    {
        [SerializeField] AudioMixer _mixer;
        Volume _postVolume;

        float _oBright;
        float _oContr;
        float _oSatur;

        float _oGenVol;
        float _oSoundVol;
        float _oMusicVol;


        private void Awake()
        {
            _postVolume = FindObjectOfType<Volume>();
        }

        private void Start()
        {
            //Tomamos los valores originales
            if (_postVolume.sharedProfile.TryGet(out ColorAdjustments color))
            {
                _oBright = color.postExposure.GetValue<float>();
                _oContr = color.contrast.GetValue<float>();
                _oSatur = color.saturation.GetValue<float>();
            }

            _mixer.GetFloat("MasterVol", out _oGenVol);
            _mixer.GetFloat("SoundVol", out _oSoundVol);
            _mixer.GetFloat("MusicVol", out _oMusicVol);

            //Aplicamos los offset de las opciones
            ApplySettings();
        }

        public void ApplySettings()
        {
            if (_postVolume.sharedProfile.TryGet(out ColorAdjustments color))
            {
                color.postExposure.Override(_oBright + GameSettings.Brightness);
                color.contrast.Override(_oContr + GameSettings.Contrast);
                color.saturation.Override(_oSatur + GameSettings.Saturation);
            }

            _mixer.SetFloat("MasterVol", _oGenVol + GameSettings.GeneralVolume);
            _mixer.SetFloat("SoundVol", _oSoundVol + GameSettings.SoundVolume);
            _mixer.SetFloat("MusicVol", _oMusicVol + GameSettings.MusicVolume);
        }
    }
}