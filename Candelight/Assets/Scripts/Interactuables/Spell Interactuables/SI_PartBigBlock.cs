using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellInteractuable
{
    public class SI_PartBigBlock : ASpellInteractuable
    {
        public TutorialBigBlock Block;
        protected override void ApplyInteraction(ASpell spell)
        {
            Block.RegisterCount();
            gameObject.SetActive(false);
        }
    }
}