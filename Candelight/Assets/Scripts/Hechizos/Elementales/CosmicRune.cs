using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class CosmicRune : AElementalRune
    {
        public CosmicRune(Mage m) : base(m)
        {
            Name = "Cósmico";
            Damage = 20f;
            m.ShowSpellChains($"{Name}: {InstructionsToString(Instructions)}\n");
        }

        //Proyectil cosmico
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

        //Cuerpo a cuerpo cosmico
        public override void MeleeActivation(Transform target)
        {

        }

        //Explosion cosmica
        public override void ExplosionActivation(Transform target)
        {

        }
        public override void ExplosionImpact(Transform target)
        {

        }

        //Potenciador cosmico
        public override void BuffActivation(Transform target)
        {

        }


    }

}