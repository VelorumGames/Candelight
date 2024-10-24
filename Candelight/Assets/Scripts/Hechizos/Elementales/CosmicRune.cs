using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class CosmicRune : AElementalRune
    {
        public CosmicRune(Mage m) : base(m)
        {
            Name = "Cosmic";
            Damage = 20f;
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
            //Atrae al enemigo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Push(200f, - (target.position - MageManager.GetPlayerTarget().position));
            }
        }
        public override void ProjectileEnd(Transform target)
        {

        }

        //Cuerpo a cuerpo cosmico
        public override void MeleeActivation(Transform target)
        {

        }
        public override void MeleeImpact(Transform target)
        {
            //Repele al enemigo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Push(100f, target.position - MageManager.GetPlayerTarget().position);
            }
        }

        //Explosion cosmica
        public override void ExplosionActivation(Transform target)
        {

        }
        public override void ExplosionImpact(Transform target)
        {
            //Repele al enemigo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Push(300f, target.position - MageManager.GetPlayerTarget().position);
            }
        }

        //Potenciador cosmico
        public override void BuffActivation(Transform target)
        {

        }


    }

}