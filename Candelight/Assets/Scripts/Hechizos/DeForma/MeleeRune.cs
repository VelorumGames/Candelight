using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class MeleeRune : AShapeRune
    {
        public event Action<Transform> OnMeleeActivation;
        public event Action<Transform> OnMeleeImpact;
        public MeleeRune(Mage m) : base(m)
        {
            Name = "Cuerpo a Cuerpo";
        }
        public override void LoadElements(Action<Transform>[] actions)
        {
            OnMeleeActivation += actions[4];
            OnMeleeImpact += actions[5];
        }

        public override void ResetElements()
        {
            OnMeleeActivation = null;
            OnMeleeImpact = null;
        }

        public override void ThrowSpell()
        {
            GameObject meleeGO = MageManager.SpawnMelee();
            if (OnMeleeActivation != null) OnMeleeActivation(MageManager.GetPlayerTarget());

            Melee melee = meleeGO.GetComponent<Melee>();
            melee.OnImpact += OnMeleeImpact;
        }
    }
}