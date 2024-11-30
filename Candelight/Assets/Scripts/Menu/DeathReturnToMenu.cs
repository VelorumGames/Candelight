using Controls;
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

namespace Menu
{
    public class DeathReturnToMenu : MonoBehaviour
    {
        public bool Active;

        [SerializeField] WorldInfo _world;
        [SerializeField] NodeInfo _currentNode;

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        private void OnEnable()
        {
            FindObjectOfType<InputManager>().LoadControls(EControlMap.UI);
            _ui.FadeToBlack(20f, () => StartCoroutine(EraseProgress()));
        }

        public void Lose()
        {
            _ui.ShowWarning(() => StartCoroutine(EraseProgress()), "Abandonarás este mundo y perderás todo lo que llevas contigo. Por suerte, tu luz perdurará tras la muerte. ¿Aceptas?");
        }

        IEnumerator EraseProgress()
        {
            _ui.ShowState(EGameState.Saving);
            
            yield return StartCoroutine(SaveSystem.Save(new SaveData(_currentNode, _world, FindObjectOfType<Inventory>())));
            SaveSystem.RestartDataOnDeath();

            _ui.ShowState(EGameState.Loading);
            ResetPermanentGameObjects();
            SaveSystem.ScoreboardIntro = true;
            SceneManager.LoadScene("ScoreboardScene");
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