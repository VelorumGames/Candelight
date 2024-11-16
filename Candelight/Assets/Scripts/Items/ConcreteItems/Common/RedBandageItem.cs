using Hechizos;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Items.ConcreteItems
{
    public class RedBandageItem : AItem
    {
        public WorldInfo World;
        bool _applied;

        //Si la vela est� a un cuarto de la vida, todos los ataques del jugador hacen un 5% m�s de da�o.

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
            if (value < 0.25 * World.MAX_CANDLE)
            {
                foreach(var rune in ARune.Spells.Values)
                {
                    if (rune is AElementalRune elRune)
                    {
                        elRune.AddDamageFactor(0.05f);
                        _applied = true;
                    }
                }
            }
            else if (_applied)
            {
                foreach (var rune in ARune.Spells.Values)
                {
                    if (rune is AElementalRune elRune)
                    {
                        elRune.RemoveDamageFactor(0.05f);
                        _applied = false;
                    }
                }
            }
        }
    }
}
