using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using World;

namespace UI
{
    public class UISeed : MonoBehaviour
    {
        WorldInfo _world;

        private void Awake()
        {
            GetComponent<TextMeshPro>().text = $"Seed: {_world.Seed}";
        }
    }
}