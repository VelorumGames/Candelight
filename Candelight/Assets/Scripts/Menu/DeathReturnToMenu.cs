using Controls;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class DeathReturnToMenu : MonoBehaviour
    {
        public bool Active;

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        private void OnEnable()
        {
            FindObjectOfType<InputManager>().LoadControls(EControlMap.UI);
            _ui.FadeToBlack(10f, EraseProgress);
        }

        //Se puede probar con una serie de ventanas para hacer algo mas artistico
        public void Lose()
        {
            _ui.ShowWarning(EraseProgress, "Abandonar�s este mundo y perder�s todo lo que llevas contigo. Por suerte, tu luz perdurar� tras la muerte. �Aceptas?");
        }

        void EraseProgress()
        {
            if (Active)
            {
                _ui.ShowState(EGameState.Loading);
                SaveSystem.RestartDataOnDeath();
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}