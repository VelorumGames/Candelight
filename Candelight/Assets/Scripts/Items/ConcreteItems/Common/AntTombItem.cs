using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class AntTombItem : AItem
    {
        int _extraSpells = 2;
        //Aumenta el número de fantasmas que emergen de la explosión fantasmal.

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Phantom", out var spell) && spell is PhantomRune phanSpell)
            {
                phanSpell.AddMaxSpellsOnExplosion(_extraSpells);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Phantom", out var spell) && spell is PhantomRune phanSpell)
            {
                phanSpell.RemoveMaxSpellsOnExplosion(_extraSpells);
            }
        }
    }
}