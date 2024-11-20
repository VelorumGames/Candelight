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
            FindObjectOfType<Inventory>().LooseItemsOnNodeExit();
            _ui.ShowWarning(() => _ui.FadeToBlack(1f, () =>
            {
                FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
                SceneManager.LoadScene("WorldManager");
            }), "¿Estás seguro de que quieres regresar? Perderás los artefactos que hayas ganado en esta zona.");
        }
    }
}