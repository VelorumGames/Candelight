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
        }

        //Proyectil electrico
        public override void ProjectileStart()
        {
        }
        public override void ProjectileUpdate()
        {

        }
        public override void ProjectileImpact()
        {

        }
        public override void ProjectileEnd()
        {

        }

        //Cuerpo a cuerpo electrico
        public override void MeleeActivation()
        {

        }

        //Explosion electrica
        public override void ExplosionActivation()
        {

        }
        public override void ExplosionImpact()
        {

        }

        //Potenciador electrico
        public override void BuffActivation()
        {

        }

    }

}