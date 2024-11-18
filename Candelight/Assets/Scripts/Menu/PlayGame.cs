using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PlayGame : MonoBehaviour
    {
        public bool PlayDirectly;
        public void InitializeGame()
        {
            SceneManager.LoadScene(PlayDirectly ? "WorldScene" : "IntroScene");
            ARune.CreateAllRunes(FindObjectOfType<Mage>());
        }
    }
}
