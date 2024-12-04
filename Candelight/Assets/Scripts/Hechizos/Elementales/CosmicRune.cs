using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class CosmicRune : AElementalRune
    {
        float _pullFactor = 0.5f;
        float _pushFactor = 0.5f;

        public CosmicRune(Mage m) : base(m)
        {
            Name = "Cosmic";
            Damage = 35f;
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
                cont.Push(200f * _pullFactor, - (target.position - MageManager.GetPlayerTarget().position));
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
                cont.Push(100f * _pushFactor, target.position - MageManager.GetPlayerTarget().position);
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
                cont.Push(300f * _pushFactor, target.position - MageManager.GetPlayerTarget().position);
            }
        }

        //Potenciador cosmico
        public override void BuffActivation(Transform target)
        {
            AddDamageFactor(0.25f);
            MageManager.ManageBuff(true, BuffReset(target));
        }
        public override IEnumerator BuffReset(Transform target)
        {
            yield return new WaitForSeconds(_buffDuration);
            AddDamageFactor(-0.25f);
            MageManager.ManageResetBuff();
        }

        public void AddPullFactor(float p) => _pullFactor += p;
        public void RemovePullFactor(float p) => _pullFactor -= p;
        public void AddPushFactor(float p) => _pushFactor += p;
        public void RemovePushFactor(float p) => _pushFactor -= p;

    }

}