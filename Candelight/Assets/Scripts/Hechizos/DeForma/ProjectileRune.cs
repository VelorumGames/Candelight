using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class ProjectileRune : AShapeRune
    {

        public event Action OnStartProjectile;
        public event Action OnUpdateProjectile;
        public event Action OnImpactProjectile;
        public event Action OnEndProjectile;

        public ProjectileRune() : base()
        {
            Name = "Proyectil";
        }
        public override void LoadElements(Action[] actions)
        {

        }
    }

}