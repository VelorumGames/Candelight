using Items;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class NodeExit : MonoBehaviour
    {
        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public void ReturnToWorld()
        {
            _ui.ShowWarning(ManageReturn, "¿Estás seguro de que quieres regresar? Perderás los artefactos que hayas ganado en esta zona.");
        }

        void ManageReturn()
        {
            _ui.Back();
            _ui.Back();

            _ui.FadeToBlack(1f, LoseItems);
        }

        void LoseItems()
        {
            FindObjectOfType<Inventory>().LoseItemsOnNodeExit();
            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}