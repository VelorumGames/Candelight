using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UISoundManager : MonoBehaviour
    {
        public AudioClip[] Inventory;
        public AudioClip Button;

        public AudioClip Move;

        public AudioClip[] Pause;

        AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void PlayButtonSound() => _audio.PlayOneShot(Button);

        public void PlayOpenInventory() => _audio.PlayOneShot(Inventory[0]);
        public void PlayCloseInventory() => _audio.PlayOneShot(Inventory[1]);

        public void PlayOpenPause() => _audio.PlayOneShot(Pause[0]);
        public void PlayClosePause() => _audio.PlayOneShot(Pause[1]);

        public void PlayMove() => _audio.PlayOneShot(Move);
    }
}