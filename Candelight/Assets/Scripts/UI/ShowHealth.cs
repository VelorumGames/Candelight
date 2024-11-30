using Player;
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

        PlayerController _cont;

        private void Awake()
        {
            _hp = GetComponent<TextMeshProUGUI>();
            _cont = FindObjectOfType<PlayerController>();

            _world.OnCandleChanged += UpdateHealth;
            if (_cont != null) _cont.OnRevive += UpdateHealth;
        }

        private void OnEnable()
        {
            UpdateHealth();
        }

        public void UpdateHealth(float hp)
        {
            _hp.text = $"HP: {_world.Candle} / {_world.MAX_CANDLE}";
        }

        void UpdateHealth() => _hp.text = $"HP: {_world.Candle} / {_world.MAX_CANDLE}";

        private void OnDestroy()
        {
            _world.OnCandleChanged -= UpdateHealth;
            if (_cont != null) _cont.OnRevive -= UpdateHealth;
        }
    }
}