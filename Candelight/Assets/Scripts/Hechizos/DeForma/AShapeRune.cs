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
        protected float FastDamageFactor;

        public AShapeRune(Mage m, int complexity, float difficulty) : base(m, complexity, difficulty) { }

        public abstract void LoadElements(Action<Transform>[] actions);
        public abstract void ResetElements();

        // Este método lanzará el hechizo basado en los elementos activos
        public abstract void ThrowSpell();

        protected float GetTotalDamage()
        {
            float avDam = 0;
            foreach (var el in MageManager.GetActiveElements()) avDam += el.GetDamage();
            avDam /= MageManager.GetActiveElements().Count;
            avDam *= FastDamageFactor;
            if (MageManager.GetActiveElements().Count > 1) avDam *= 0.75f;
            return avDam;
        }

        public void SetFastDamageFactor(float dam) => FastDamageFactor = dam;
        public void ResetFastDamageFactor() => FastDamageFactor = 1f;
        public float GetFastDamageFactor() => FastDamageFactor;
    }
}
