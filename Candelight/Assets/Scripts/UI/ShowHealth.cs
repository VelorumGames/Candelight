using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using World;

namespace UI
{
    public class ShowHealth : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        TextMeshProUGUI _hp;

        private void Awake()
        {
            _hp = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _hp.text = $"HP: {_world.Candle} / {_world.MAX_CANDLE}";
        }
    }
}