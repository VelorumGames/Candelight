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
        public ExplosionRune(Mage m) : base(m)
        {
            Name = "Explosión";
        }
        public override void LoadElements(Action[] actions)
        {
            OnExplosionActivation += actions[5];
            OnExplosionImpact += actions[6];
        }

        public override void ResetElements()
        {
            OnExplosionActivation = null;
            OnExplosionImpact = null;
        }

        public override void ThrowSpell()
        {
            GameObject expl = MageManager.SpawnExplosion();
            if (OnExplosionActivation != null) OnExplosionActivation();
        }
    }
}