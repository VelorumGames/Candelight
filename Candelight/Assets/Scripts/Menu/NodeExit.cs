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
            Debug.Log("AAAA");

            _ui.ShowWarning(ManageReturn, "�Est�s seguro de que quieres regresar? Perder�s los artefactos que hayas ganado en esta zona.");
        }

        void ManageReturn()
        {
            Debug.Log("BBBB");
            _ui.FadeToBlack(1f, LoseItems);
        }

        void LoseItems()
        {
            Debug.Log("CCCC");

            FindObjectOfType<Inventory>().LooseItemsOnNodeExit();
            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}