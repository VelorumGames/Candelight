using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class MeleeRune : AShapeRune
    {
        public event Action<Transform> OnMeleeActivation;
        public MeleeRune(Mage m) : base(m)
        {
            Name = "Cuerpo a Cuerpo";
            m.ShowSpellChains($"{Name}: {InstructionsToString(Instructions)}\n");
        }
        public override void LoadElements(Action<Transform>[] actions)
        {
            OnMeleeActivation += actions[4];
        }

        public override void ResetElements()
        {
            OnMeleeActivation = null;
        }

        public override void ThrowSpell()
        {
            GameObject meleeGO = MageManager.SpawnMelee();
            if (OnMeleeActivation != null) OnMeleeActivation(null);
        }
    }
}