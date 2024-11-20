using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ReturnToMenu : MonoBehaviour
    {
        public bool Warning;
        public void Return()
        {
            if (Warning)
            {
                UIManager ui = FindObjectOfType<UIManager>();

                if (ui != null)
                {
                    ui.ShowWarning(LoadScene, "�Est�s seguro de que quieres regresar al men� principal?");
                }
            }
            else
            {
                FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
                SceneManager.LoadScene("MenuScene");
            }
        }

        void LoadScene()
        {
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
            SceneManager.LoadScene("MenuScene");
        }
    }
}