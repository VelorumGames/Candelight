using DG.Tweening.Core.Easing;
using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class PhantomRune : AElementalRune
    {
        int _maxSpellsOnExplosion = 5;
        float _slowness = 0.5f;

        public PhantomRune(Mage m) : base(m)
        {
            Name = "Phantom";
            Damage = 25f;
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
                cont.Slow(_slowness, 3f);
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
            for (int i = 0; i < _maxSpellsOnExplosion; i++)
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
            MageManager.SetExtraProjSpeed(_buffedSpeed);
            MageManager.ManageBuff(true, BuffReset(target));
            AddDamageFactor(0.25f);
        }
        public override IEnumerator BuffReset(Transform target)
        {
            yield return new WaitForSeconds(_buffDuration);
            MageManager.SetExtraProjSpeed(1);
            AddDamageFactor(-0.25f);
            MageManager.ManageResetBuff();
        }

        public void SetMaxSpellsOnExplosion(int n) => _maxSpellsOnExplosion = n;
        public void AddMaxSpellsOnExplosion(int n) => _maxSpellsOnExplosion += n;
        public void RemoveMaxSpellsOnExplosion(int n) => _maxSpellsOnExplosion -= n;
        public int GetMaxSpellsOnExplosion(int n) => _maxSpellsOnExplosion;

        public void AddSlowness(float slow) => _slowness *= slow;
        public void RemoveSlowness(float slow) => _slowness /= slow;

    }
}
