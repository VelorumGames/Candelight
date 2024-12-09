using Hechizos.Elementales;
using SpellInteractuable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public abstract class ASpell : MonoBehaviour
    {
        public AElementalRune[] Elements;
        public AudioClip[] _elementSounds;

        protected AudioSource Audio;

        public bool AffectsPlayer;

        protected void Awake()
        {
            Audio = GetComponent<AudioSource>();
        }

        protected void OnEnable()
        {
            Elements = Mage.Instance.GetActiveElements().ToArray();
            if (!AffectsPlayer) RegisterTypes(Elements);
            if (_elementSounds.Length > 0) PlayElementSound();
        }

        void PlayElementSound()
        {
            switch(Elements[0].Name)
            {
                case "Fire":
                    Audio.PlayOneShot(_elementSounds[0]);
                    break;
                case "Electric":
                    Audio.PlayOneShot(_elementSounds[1]);
                    break;
                case "Cosmic":
                    Audio.PlayOneShot(_elementSounds[2]);
                    break;
                case "Phantom":
                    Audio.PlayOneShot(_elementSounds[3]);
                    break;
            }
        }

        protected bool FindElement(string elName)
        {
            foreach (var el in Elements)
            {
                if (el.Name == elName) return true;
            }
            return false;
        }

        protected abstract void RegisterTypes(AElementalRune[] runes);

    }
}