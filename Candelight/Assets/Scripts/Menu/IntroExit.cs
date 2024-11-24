using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class IntroExit : MonoBehaviour
    {
        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public void Exit()
        {
            FindObjectOfType<UIManager>().ShowWarning(ManageSkip, "�Est�s seguro de que quieres saltarte el tutorial?");
        }

        void ManageSkip()
        {
            if (ARune.FindSpell("Fire", out var spell)) spell.Activate(true);
            if (ARune.FindSpell("Electric", out spell)) spell.Activate(true);
            if (ARune.FindSpell("Projectile", out spell)) spell.Activate(true);

            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}