using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos;
using Hechizos.Elementales;
using System;

namespace Hechizos.DeForma
{
    public abstract class AShapeRune : ARune
    {
        protected Action<Transform>[] ElementActions;

        public AShapeRune(Mage m, int complexity, float difficulty) : base(m, complexity, difficulty) { }

        public abstract void LoadElements(Action<Transform>[] actions);
        public abstract void ResetElements();

        // Este m�todo lanzar� el hechizo basado en los elementos activos
        public abstract void ThrowSpell();

        protected float GetTotalDamage()
        {
            float avDam = 0;
            foreach (var el in MageManager.GetActiveElements()) avDam += el.GetDamage();
            avDam /= MageManager.GetActiveElements().Count;
            if (MageManager.GetActiveElements().Count > 1) avDam *= 0.75f;
            return avDam;
        }

    }
}
