using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellInteractuable
{
    public abstract class ASpellInteractuable : MonoBehaviour
    {
        [SerializeField] protected EElements Element;
        [SerializeField] protected bool IgnoreElements;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Spell") && other.TryGetComponent<ASpell>(out var scriptSpell))
            {
                foreach (var elem in scriptSpell.Elements)
                {
                    if (elem.Name == Element.ToString() || IgnoreElements)
                    {
                        ApplyInteraction(scriptSpell);
                        return;
                    }
                }
            }
        }

        protected abstract void ApplyInteraction(ASpell spell);

        public void ForceApplyInteraction(ASpell spell) => ApplyInteraction(spell);
    }

    public enum EElements
    {
        Fire,
        Electric,
        Cosmic,
        Phantom
    }

}