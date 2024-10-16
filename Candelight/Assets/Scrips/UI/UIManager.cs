using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GUI.Label(new Rect(10, 10, 200, 200), $"FPS: {1.0f / Time.deltaTime}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName}");
        }
    }
}