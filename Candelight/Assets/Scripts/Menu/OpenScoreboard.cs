using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class OpenScoreboard : MonoBehaviour
    {
        public void Open()
        {
            SceneManager.LoadScene("ScoreboardScene");
        }
    }
}
