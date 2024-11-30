using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class OpenScoreboard : MonoBehaviour
    {
        public void Open()
        {
            if (GameSettings.Online)
            {
                FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
                SceneManager.LoadScene("ScoreboardScene");
            }
            else
            {
                FindObjectOfType<UIManager>().ShowTutorial("Las puntuaciones no est�n disponibles en el Modo Sin Conexi�n.");
            }
        }
    }
}
