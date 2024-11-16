using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class CopperHeartItem : AItem
    {
        float _extraElectric = 0.15f;
        float _fireReduction = 0.15f;

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Electric", out var spell) && spell is ElectricRune elecSpell)
            {
                elecSpell.AddDamageFactor(_extraElectric);
            }

            if (ARune.FindSpell("Fire", out var firespell) && firespell is FireRune fireSpell)
            {
                fireSpell.RemoveDamageFactor(_fireReduction);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Electric", out var spell) && spell is ElectricRune elecSpell)
            {
                elecSpell.RemoveDamageFactor(_extraElectric);
            }

            if (ARune.FindSpell("Fire", out var firespell) && firespell is FireRune fireSpell)
            {
                fireSpell.AddDamageFactor(_fireReduction);
            }
        }
    }
}