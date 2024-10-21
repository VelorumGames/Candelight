using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class ExplosionRune : AShapeRune
    {
        public event Action OnExplosionActivation;
        public event Action OnExplosionImpact;
        public ExplosionRune() : base()
        {
            Name = "Explosi�n";
        }
        public override void LoadElements(Action[] actions)
        {

        }
    }
}