using Cameras;
using Controls;
using DG.Tweening;
using Hechizos;
using Items;
using Music;
using Player;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Rest
{
    public class Rest : MonoBehaviour
    {
        [SerializeField] float _transitionDuration = 5f;
        [SerializeField] WorldInfo _world;

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public void StartRest()
        {
            _ui.ShowWarning(() => StartCoroutine(LoadRest()), "¿Estás seguro de que quieres descansar? Se guardará tu progreso y volverás al menú principal.");
        }

        IEnumerator LoadRest()
        {
            yield return StartCoroutine(SavePlayerData());

            _ui.Back();
            _ui.Back();

            if (_world.CompletedNodes > 5)
            {
                FindObjectOfType<CameraManager>().SetActiveCamera(1, _transitionDuration);
                _ui.FadeToWhite(_transitionDuration, Ease.InCirc, () =>
                {
                    ResetPermanentGameObjects();
                    _ui.ShowState(EGameState.Loading);
                    SceneManager.LoadScene("ScoreboardScene");
                });
            }
            else
            {
                FindObjectOfType<CameraManager>().SetActiveCamera(1, _transitionDuration);
                _ui.FadeToBlack(_transitionDuration, () =>
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
            yield return StartCoroutine(SaveSystem.Save(new SaveData(FindObjectOfType<WorldManager>().CurrentNodeInfo, _world, FindObjectOfType<Inventory>())));

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

            foreach (var item in FindObjectsOfType<AItem>(true))
            {
                Destroy(item.gameObject);
            }

            LuciernagaController[] luciernagas = FindObjectsOfType<LuciernagaController>();
            foreach(var l in luciernagas)
            {
                if (l != null) Destroy(l.gameObject);
            }
        }
    }
}