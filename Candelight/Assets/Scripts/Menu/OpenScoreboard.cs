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
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
            SceneManager.LoadScene("ScoreboardScene");
        }
    }
}
