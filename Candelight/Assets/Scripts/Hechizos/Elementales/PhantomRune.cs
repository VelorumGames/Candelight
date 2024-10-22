using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class PhantomRune : AElementalRune
    {
        public PhantomRune(Mage m) : base(m)
        {
            Name = "Fantasmal";
            Damage = 8f;
        }

        //Proyectil fantasmal
        public override void ProjectileStart(Transform target)
        {
        }
        public override void ProjectileUpdate(Transform target)
        {

        }
        public override void ProjectileImpact(Transform target)
        {

        }
        public override void ProjectileEnd(Transform target)
        {

        }

        //Cuerpo a cuerpo fantasmal
        public override void MeleeActivation(Transform target)
        {

        }

        //Explosion fantasmal
        public override void ExplosionActivation(Transform target)
        {

        }
        public override void ExplosionImpact(Transform target)
        {

        }

        //Potenciador fantasmal
        public override void BuffActivation(Transform target)
        {

        }

    }
}
