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
            _world.OnCandleChanged += UpdateHealth;
        }

        void UpdateHealth(float hp)
        {
            _hp.text = $"HP: {hp} / {_world.MAX_CANDLE}";
        }

        private void OnDisable()
        {
            _world.OnCandleChanged -= UpdateHealth;
        }
    }
}