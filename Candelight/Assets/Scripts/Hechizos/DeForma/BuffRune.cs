using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class BuffRune : AShapeRune
    {
        public event Action OnBuffActivation;
        public BuffRune(Mage m) : base(m)
        {
            Name = "Potenciación";
        }
        public override void LoadElements(Action[] actions)
        {
            OnBuffActivation += actions[7];
        }

        public override void ResetElements()
        {
            OnBuffActivation = null;
        }

        public override void ThrowSpell()
        {
            
        }
    }
}