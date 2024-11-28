using Hechizos;
using Items;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Menu
{
    public class PlayGame : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        Inventory _inv;

        public bool PlayDirectly;
        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
            _inv = FindObjectOfType<Inventory>();
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

            _world.Candle = _world.MAX_CANDLE;
            _world.CompletedIds.Clear();
            _inv.ResetInventory();

            ARune.CreateAllRunes(FindObjectOfType<Mage>());

            _ui.ShowState(EGameState.Loading);
            if (PlayDirectly || !GameSettings.Tutorial)
            {
                ManageSkip();
                SceneManager.LoadScene("WorldScene");
            }
            else
            {
                SceneManager.LoadScene("IntroScene");
            }
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
