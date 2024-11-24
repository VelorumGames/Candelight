using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ManageCredits : MonoBehaviour
    {
        public void Credits()
        {
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
            SceneManager.LoadScene("CreditsScene");
        }
    }
}