using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class PlayClipOnTrigger : MonoBehaviour
    {
        public int Id;
        public AudioClip Clip;
        public bool Once;

        bool _activated = true;

        MusicManager _music;

        private void Awake()
        {
            _music = FindObjectOfType<MusicManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_activated)
            {
                _music.SetLooping(Id, false);
                _music.PlayMusic(Id, Clip);
                if (_music.GetCurrentVolume(Id) < 0.1f) _music.ChangeVolumeTo(Id, 0.5f, 0.5f);
                _activated = !Once;
            }
        }
    }
}