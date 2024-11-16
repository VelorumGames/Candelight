using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class StormInABottleItem : AItem
    {

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Electric", out var spell) && spell is ElectricRune elSpell)
            {
                elSpell.ConstantBuff();
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Electric", out var spell) && spell is ElectricRune elSpell)
            {
                elSpell.ConstantBuffReset();
            }
        }
    }
}
