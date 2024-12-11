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
            _ui.ShowWarning(ManageSkip, "¿Estás seguro de que quieres saltarte el tutorial?");
        }

        void ManageSkip()
        {
            Debug.Log("Elemento de inicio: " + Upgrades.StartElement.ToString());
            if (ARune.FindSpell(Upgrades.StartElement.ToString(), out var spell)) spell.Activate(true);
            //if (ARune.FindSpell("Fire", out spell)) spell.Activate(true);
            if (ARune.FindSpell("Electric", out spell)) spell.Activate(true);
            if (ARune.FindSpell("Projectile", out spell)) spell.Activate(true);

            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}