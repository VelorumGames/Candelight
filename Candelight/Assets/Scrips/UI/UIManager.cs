using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 200), $"FPS: {1.0f / Time.deltaTime}");
        }
    }
}