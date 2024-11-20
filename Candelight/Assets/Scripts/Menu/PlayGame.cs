using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PlayGame : MonoBehaviour
    {
        public bool PlayDirectly;
        UIManager _ui;

        private void Awake()
        {
            _ui = GetComponent<UIManager>();
        }

        public void InitializeGame()
        {
            if (SaveSystem.ExistsPreviousGame())
            {
                _ui.ShowWarning(StartGame, "Al iniciar una nueva partida desaparecerán todos tus datos de tu partida anterior. ¿Deseas continuar?");
            }
            else
            {
                StartGame();
            }
        }

        void StartGame()
        {
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
            SceneManager.LoadScene(PlayDirectly ? "WorldScene" : "IntroScene");
            ARune.CreateAllRunes(FindObjectOfType<Mage>());
        }
    }
}
