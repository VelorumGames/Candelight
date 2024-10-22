using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hechizos.Elementales
{
    public class ElectricRune : AElementalRune
    {
        public ElectricRune(Mage m) : base(m)
        {
            Name = "Electricidad";
            Damage = 15f;
        }

        //Proyectil electrico
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

        //Cuerpo a cuerpo electrico
        public override void MeleeActivation(Transform target)
        {

        }

        //Explosion electrica
        public override void ExplosionActivation(Transform target)
        {

        }
        public override void ExplosionImpact(Transform target)
        {

        }

        //Potenciador electrico
        public override void BuffActivation(Transform target)
        {

        }

    }

}