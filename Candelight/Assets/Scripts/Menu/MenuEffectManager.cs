using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuEffectManager : MonoBehaviour
    {
        [SerializeField] ParticleSystem _transitionParticles;
        [SerializeField] RectTransform _menuContainer;
        [SerializeField] float _duration;

        string _sceneName;
        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public void StartGame(string SceneName)
        {
            _sceneName = SceneName;

            StartCoroutine(ManageCinematic());
        }

        IEnumerator ManageCinematic()
        {
            _transitionParticles.Play();
            yield return new WaitForSeconds(_duration * 0.5f);
            _ui.FadeToBlack(_duration, ChangeScene);
        }

        void ChangeScene()
        {
            Debug.Log("CAMBIOOO DE ESCENA");
            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene(_sceneName);
        }
    }
}