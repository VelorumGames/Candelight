using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using World;

namespace UI
{
    public class UISeed : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;

        private void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = $"Seed: {_world.Seed}";
        }
    }
}