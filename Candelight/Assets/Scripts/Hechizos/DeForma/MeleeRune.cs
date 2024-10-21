using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class MeleeRune : AShapeRune
    {
        public event Action OnMeleeActivation;
        public MeleeRune() : base()
        {
            Name = "Cuerpo a Cuerpo";
        }
        public override void LoadElements(Action[] actions)
        {

        }
    }

}