using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellInteractuable
{
    public class IntroBlockInter : ASpellInteractuable
    {
        protected override void ApplyInteraction(ASpell spell)
        {
            GetComponent<IntroBlock>().ResetBlock();
        }
    }
}