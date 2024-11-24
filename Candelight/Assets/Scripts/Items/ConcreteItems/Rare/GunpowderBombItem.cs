using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class GunpowderBombItem : AItem
    {
        //El potenciador fantasmal incrementa un 5% más la velocidad de todos los proyectiles fantasmales.

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Fire", out var spell) && spell is FireRune fireSpell)
            {
                fireSpell.SetExplosionOnImpact(true);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Fire", out var spell) && spell is FireRune fireSpell)
            {
                fireSpell.SetExplosionOnImpact(false);
            }
        }
    }
}