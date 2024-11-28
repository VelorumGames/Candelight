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
            _ui.FadeToBlack(20f, EraseProgress);
        }

        public void Lose()
        {
            _ui.ShowWarning(EraseProgress, "Abandonarás este mundo y perderás todo lo que llevas contigo. Por suerte, tu luz perdurará tras la muerte. ¿Aceptas?");
        }

        void EraseProgress()
        {
            _ui.ShowState(EGameState.Loading);
            SaveSystem.RestartDataOnDeath();
            SceneManager.LoadScene("MenuScene");
        }
    }
}