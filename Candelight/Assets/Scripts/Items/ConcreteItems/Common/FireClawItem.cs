using Hechizos;
using Hechizos.Elementales;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Items.ConcreteItems
{
    public class FireClawItem : AItem
    {
        //El impacto de los ataques de fuego hace un 5% más de daño. 

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Fire", out var spell) && spell is AElementalRune fireSpell)
            {
                fireSpell.AddDamageFactor(0.05f);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Fire", out var spell) && spell is AElementalRune fireSpell)
            {
                fireSpell.RemoveDamageFactor(0.05f);
            }
        }
    }
}