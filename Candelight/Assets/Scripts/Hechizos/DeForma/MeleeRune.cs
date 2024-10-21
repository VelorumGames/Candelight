using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class MeleeRune : AShapeRune
    {
        public event Action OnMeleeActivation;
        public MeleeRune(Mage m) : base(m)
        {
            Name = "Cuerpo a Cuerpo";
        }
        public override void LoadElements(Action[] actions)
        {
            OnMeleeActivation += actions[4];
        }

        public override void ResetElements()
        {
            OnMeleeActivation = null;
        }

        public override void ThrowSpell()
        {
            if (OnMeleeActivation != null) OnMeleeActivation();
        }
    }
}