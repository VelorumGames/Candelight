using Dialogues;
using Events;
using Hechizos;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace SpellInteractuable
{
    public class SI_AnchorBarrier : ASpellInteractuable
    {
        [SerializeField] bool _active;
        [SerializeField] int _atCount;
        [SerializeField] int _attackLimit;
        protected override void ApplyInteraction(ASpell spell)
        {
            if (_active)
            {
                if(_atCount++ >= _attackLimit)
                {
                    //GetComponent<AnchorManager>().OpenAnchor();
                    //_active = false;
                    Destroy(gameObject);
                }
            }
        }

        public bool Activate(bool b) => _active = b;
    }
}