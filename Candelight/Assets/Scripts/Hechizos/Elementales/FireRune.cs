using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class FireRune : AElementalRune
    {
        public FireRune(Mage m) : base(m)
        {
            Name = "Fuego";
            Damage = 100f;
            m.SetInitialElement(this);
        }

        //Proyectil de fuego
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

        //Cuerpo a cuerpo de fuego
        public override void MeleeActivation(Transform target)
        {

        }

        //Explosion de fuego
        public override void ExplosionActivation(Transform target)
        {

        }
        public override void ExplosionImpact(Transform target)
        {

        }

        //Potenciador de fuego
        public override void BuffActivation(Transform target)
        {

        }

    }

}