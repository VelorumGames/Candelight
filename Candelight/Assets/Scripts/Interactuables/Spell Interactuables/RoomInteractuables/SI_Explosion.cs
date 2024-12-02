using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellInteractuable
{
    public class SI_Explosion : ASpellInteractuable
    {
        [SerializeField] GameObject _explosion;

        protected override void ApplyInteraction(ASpell spell)
        {
            GameObject expl = Instantiate(_explosion, transform.position, Quaternion.identity);
        }
    }
}
