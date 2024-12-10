using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Items.ConcreteItems
{
    public class StormInABottleItem : AItem
    {

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Electric", out var spell) && spell is ElectricRune elSpell)
            {
                elSpell.ConstantBuff();
                FindObjectOfType<PlayerController>().RemoveSpeedFactor(0.1f);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Electric", out var spell) && spell is ElectricRune elSpell)
            {
                elSpell.ConstantBuffReset();
                FindObjectOfType<PlayerController>().AddSpeedFactor(0.1f);
            }
        }
    }
}
