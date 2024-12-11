using Cameras;
using Controls;
using DG.Tweening;
using Hechizos;
using Items;
using Menu;
using Music;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace UI.Ads
{
    public class DeathAd : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        public NodeInfo CurrentNodeInfo;
        UIManager _ui;
        [SerializeField] GameObject _ad;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        private void OnEnable()
        {
            if (!GameSettings.CanRevive) gameObject.SetActive(false);
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

            _ui.ShowWarning(() => StartCoroutine(LoadRest()), "Para poder continuar tu partida regresarás al menú principal.", "Ok");
            /*
            GameSettings.CanRevive = false;
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Level);*/
            FindObjectOfType<PlayerSounds>().PlayReviveSound();
            
        }

        IEnumerator LoadRest()
        {
            yield return StartCoroutine(SavePlayerData());

            _ui.Back();
            _ui.Back();

            if (_world.CompletedNodes > 5)
            {
                _ui.FadeToWhite(3f, Ease.InCirc, () =>
                {
                    ResetPermanentGameObjects();
                    _ui.ShowState(EGameState.Loading);
                    SceneManager.LoadScene("ScoreboardScene");
                });
            }
            else
            {
                _ui.FadeToBlack(3f, () =>
                {
                    ResetPermanentGameObjects();
                    _ui.ShowState(EGameState.Loading);
                    SceneManager.LoadScene("ScoreboardScene");
                });
            }
        }

        IEnumerator SavePlayerData()
        {
            SaveSystem.GenerateNewPlayerData(_world);
            SaveSystem.ScoreboardIntro = true;
            _ui.ShowState(EGameState.Saving);
            yield return StartCoroutine(SaveSystem.Save(new SaveData(CurrentNodeInfo, _world, FindObjectOfType<Inventory>())));

            Debug.Log("Se termina de guardar");
        }

        void ResetPermanentGameObjects()
        {
            Destroy(_world.World);
            Destroy(FindObjectOfType<PlayerController>().gameObject);
            Destroy(FindObjectOfType<MusicManager>().gameObject);
            Destroy(FindObjectOfType<InputManager>().gameObject);
            Destroy(FindObjectOfType<Mage>().gameObject);
            Destroy(FindObjectOfType<Inventory>().gameObject);
        }
    }
}