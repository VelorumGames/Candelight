using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class BuffRune : AShapeRune
    {
        public event Action<Transform> OnBuffActivation;
        public BuffRune(Mage m) : base(m)
        {
            Name = "Potenciación";
            m.ShowSpellChains($"{Name}: {InstructionsToString(Instructions)}\n");
        }
        public override void LoadElements(Action<Transform>[] actions)
        {
            OnBuffActivation += actions[7];
        }

        public override void ResetElements()
        {
            OnBuffActivation = null;
        }

        public override void ThrowSpell()
        {
            GameObject buffGO = MageManager.SpawnBuff();
            if (OnBuffActivation != null) OnBuffActivation(MageManager.transform);
        }
    }
}