using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hechizos.Elementales
{
    public class ElectricRune : AElementalRune
    {
        public ElectricRune(Mage m) : base(m)
        {
            Name = "Electric";
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
            //Queda paralizado durante cierto tiempo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Paralize(3f);
            }
        }
        public override void ProjectileEnd(Transform target)
        {

        }

        //Cuerpo a cuerpo electrico
        public override void MeleeActivation(Transform target)
        {

        }
        public override void MeleeImpact(Transform target)
        {
            //Queda paralizado durante cierto tiempo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Paralize(1.5f);
            }
        }

        //Explosion electrica
        public override void ExplosionActivation(Transform target)
        {

        }
        public override void ExplosionImpact(Transform target)
        {
            //Queda paralizado durante cierto tiempo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.Paralize(5f);
            }
        }

        //Potenciador electrico
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

        public void ConstantBuff()
        {
            MageManager.ManageBuff(false, BuffReset(null));
            AddDamageFactor(0.25f);
        }
        public void ConstantBuffReset()
        {
            AddDamageFactor(-0.25f);
            MageManager.ManageResetBuff();
        }
    }
}