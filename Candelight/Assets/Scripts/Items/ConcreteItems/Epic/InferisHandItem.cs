using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class InferisHandItem : AItem
    {
        float _extraSlowness = 0.15f;

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Phantom", out var spell) && spell is PhantomRune phSpell)
            {
                phSpell.AddSlowness(_extraSlowness);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Phantom", out var spell) && spell is PhantomRune phSpell)
            {
                phSpell.RemoveSlowness(_extraSlowness);
            }
        }
    }
}