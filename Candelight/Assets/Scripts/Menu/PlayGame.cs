using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PlayGame : MonoBehaviour
    {
        public void InitializeGame()
        {
            SceneManager.LoadScene("WorldScene");
            ARune.CreateAllRunes(FindObjectOfType<Mage>());
        }
    }
}
