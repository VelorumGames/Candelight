using Hechizos.Elementales;
using SpellInteractuable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public abstract class ASpell : MonoBehaviour
    {
        public AElementalRune[] Elements;

        protected void OnEnable()
        {
            Elements = Mage.Instance.GetActiveElements().ToArray();
            RegisterTypes(Elements);
        }

        protected abstract void RegisterTypes(AElementalRune[] runes);

    }
}