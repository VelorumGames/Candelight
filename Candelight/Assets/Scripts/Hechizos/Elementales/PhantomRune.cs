using DG.Tweening.Core.Easing;
using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class PhantomRune : AElementalRune
    {
        public PhantomRune(Mage m) : base(m)
        {
            Name = "Phantom";
            Damage = 8f;
        }

        //Proyectil fantasmal
        public override void ProjectileStart(Transform target)
        {
            //Se pone drag al proyectil y se localiza el enemigo mas cercano (y se almacena individualmente en cada proyectil)
            MageManager.SetProjectileDrag(1f);
            if (MageManager.TryFindClosestEnemy(out var enemy))
            {
                MageManager.SetProjectileTarget(enemy, 20000f);
            }
            
        }
        public override void ProjectileUpdate(Transform target)
        {

        }
        public override void ProjectileImpact(Transform target)
        {
            //Ralentiza al enemigo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Slow(0.5f, 3f);
            }
        }
        public override void ProjectileEnd(Transform target)
        {

        }

        //Cuerpo a cuerpo fantasmal
        public override void MeleeActivation(Transform target)
        {

        }
        public override void MeleeImpact(Transform target)
        {
            //Ralentiza al enemigo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Slow(0.5f, 3f);
            }
        }

        //Explosion fantasmal
        public override void ExplosionActivation(Transform target)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject projGO = MageManager.SpawnProjectileWithRandomDirection();
                ProjectileStart(target);

                Projectile proj = projGO.GetComponent<Projectile>();
                proj.OnUpdate += ProjectileUpdate;
                proj.OnImpact += ProjectileImpact;
                proj.OnEnd += ProjectileEnd;

                float avDam = 0;
                foreach (var el in MageManager.GetActiveElements()) avDam += el.GetDamage();
                avDam /= MageManager.GetActiveElements().Count;
                proj.Damage = avDam;
            }
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
