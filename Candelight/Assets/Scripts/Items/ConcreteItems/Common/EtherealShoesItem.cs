using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;
using Player;

namespace Items.ConcreteItems
{
    public class EtherealShoesItem : AItem
    {
        public WorldInfo World;
        bool _applied;

        PlayerController _player;

        //Si la vela tiene más de la mitad vida, la velocidad del jugador aumenta un 10%.

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        protected override void ApplyProperty()
        {
            World.OnCandleChanged += CandleChanged;
        }

        protected override void ResetProperty()
        {
            World.OnCandleChanged -= CandleChanged;
        }

        void CandleChanged(float value)
        {
            if (value > 0.5 * World.MAX_CANDLE)
            {
                _player.AddSpeedFactor(0.1f);
                _applied = true;
            }
            else if (_applied)
            {
                _player.RemoveSpeedFactor(0.1f);
                _applied = false;
            }
        }
    }
}
