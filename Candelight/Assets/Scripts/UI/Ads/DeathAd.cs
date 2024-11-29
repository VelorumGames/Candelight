using Controls;
using Menu;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace UI.Ads
{
    public class DeathAd : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        UIManager _ui;
        [SerializeField] GameObject _ad;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public void Save()
        {
            FindObjectOfType<DeathReturnToMenu>().Active = false;
            _ui.InterruptFade();
            _ui.FadeToWhite(1f, LoadAd);
        }

        void LoadAd()
        {
            //Se carga el anuncio
            _ad.SetActive(true);
            _ui.InterruptFade();

            //Debug. Deberia estar desactivado
            //PostAd();
        }

        /// <summary>
        /// Funcion a la que se llama una vez el anuncio ha terminado
        /// </summary>
        public void PostAd()
        {
            _ad.SetActive(false);
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Level);
            FindObjectOfType<PlayerController>().Revive();
        }
    }
}