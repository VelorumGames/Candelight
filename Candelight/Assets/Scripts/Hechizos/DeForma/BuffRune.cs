using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class BuffRune : AShapeRune
    {
        float NewDamageFactor = 1f;
        public event Action<Transform> OnBuffActivation;
        public BuffRune(Mage m) : base(m)
        {
            Name = "Buff";
        }
        public override void LoadElements(Action<Transform>[] actions)
        {
            OnBuffActivation += actions[8];
        }

        public override void ResetElements()
        {
            OnBuffActivation = null;
        }

        public override void ThrowSpell()
        {
            GameObject buffGO = MageManager.SpawnBuff();
            if (OnBuffActivation != null) OnBuffActivation(MageManager.GetPlayerTarget());

            foreach(var el in MageManager.GetActiveElements())
            {
                el.SetDamageFactor(NewDamageFactor);
            }
        }

        public float GetNewFactor() => NewDamageFactor;
        public void SetNewFactor(float newFactor) => NewDamageFactor = newFactor;
    }
}