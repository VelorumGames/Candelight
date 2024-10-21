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
        protected Action[] ElementActions;

        public AShapeRune(Mage m) : base(m, 3, 0.5f) { }

        public abstract void LoadElements(Action[] actions);
        public abstract void ResetElements();

        // Este método lanzará el hechizo basado en los elementos activos
        public abstract void ThrowSpell();

    }
}
