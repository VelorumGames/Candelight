using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class ExplosionRune : AShapeRune
    {
        public event Action<Transform> OnExplosionActivation;
        public event Action<Transform> OnExplosionImpact;
        public ExplosionRune(Mage m) : base(m)
        {
            Name = "Explosión";
        }
        public override void LoadElements(Action<Transform>[] actions)
        {
            OnExplosionActivation += actions[6];
            OnExplosionImpact += actions[7];
        }

        public override void ResetElements()
        {
            OnExplosionActivation = null;
            OnExplosionImpact = null;
        }

        public override void ThrowSpell()
        {
            GameObject explGO = MageManager.SpawnExplosion();
            if (OnExplosionActivation != null)
            {
                OnExplosionActivation(MageManager.GetPlayerTarget());
            }

            Explosion expl = explGO.GetComponent<Explosion>();
            expl.OnImpact += OnExplosionImpact;

            float avDam = 0;
            foreach (var el in MageManager.GetActiveElements()) avDam += el.Damage;
            avDam /= MageManager.GetMaxElements();
            expl.Damage = avDam;
        }
    }
}