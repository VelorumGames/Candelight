using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class DeathWhistleItem : AItem
    {
        float _extraSpeed = 0.05f;
        //El potenciador fantasmal incrementa un 5% más la velocidad de todos los proyectiles fantasmales.

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Phantom", out var spell) && spell is PhantomRune phanSpell)
            {
                phanSpell.AddBuffProjSpeed(_extraSpeed);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Phantom", out var spell) && spell is PhantomRune phanSpell)
            {
                phanSpell.RemoveBuffProjSpeed(_extraSpeed);
            }
        }
    }
}