using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellInteractuable
{
    public abstract class ASpellInteractuable : MonoBehaviour
    {
        [SerializeField] protected EElements Element;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Spell") && other.TryGetComponent<ASpell>(out ASpell scriptSpell))
            {
                foreach (var elem in scriptSpell.Elements) 
                {
                    if (elem.Name == Element.ToString())
                    {
                        ApplyInteraction();
                        return;
                    }
                       
                   
                }
            }

        }


        protected abstract void ApplyInteraction();
       

    }



    public enum EElements
    {
        Fire,
        Electric,
        Cosmic,
        Phantom
    }

}