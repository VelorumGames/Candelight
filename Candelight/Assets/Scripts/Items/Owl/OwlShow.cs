using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class OwlShow : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(GameSettings.Owl);
        }
    }
}