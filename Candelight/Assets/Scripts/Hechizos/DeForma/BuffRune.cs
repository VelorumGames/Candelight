using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class BuffRune : AShapeRune
    {
        public event Action OnBuffActivation;
        public BuffRune() : base()
        {
            Name = "Potenciación";
        }
        public override void LoadElements(Action[] actions)
        {
            
        }
    }
}