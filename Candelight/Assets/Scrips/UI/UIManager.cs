using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public string NextNodeName;
        public string ActualNodeName;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 50), $"FPS: {1.0f / Time.deltaTime}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName}");
            if (GUI.Button(new Rect(200, 10, 150, 20), "WORLD SCENE")) SceneManager.LoadScene("WorldScene");
            if (GUI.Button(new Rect(400, 10, 150, 20), "LEVEL SCENE")) SceneManager.LoadScene("LevelScene");
            if (GUI.Button(new Rect(600, 10, 150, 20), "CALM SCENE")) SceneManager.LoadScene("CalmScene");
        }
    }
}