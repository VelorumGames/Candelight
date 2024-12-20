using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public class FireRune : AElementalRune
    {
        bool _bombEffect;
        int _count = 0;

        public FireRune(Mage m) : base(m)
        {
            Name = "Fire";
            Damage = 20f;
            Activate(true);
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
            //Debug.Log("AAAAAAA: " + target);

            //Queda haciendo dano durante cierto tiempo
            if (target != null && target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.RecieveTemporalDamage(Damage * 0.25f, 5f, 1f);
                cont.Burn(5f);
            }
            else if (target.transform.parent != null && target.transform.parent.TryGetComponent<EnemyController>(out cont))
            {
                target = target.transform.parent;
                cont.RecieveTemporalDamage(Damage * 0.25f, 5f, 1f);
                cont.Burn(5f);
            }

            if (_bombEffect)
            {
                MageManager.SpawnCustomExplosion(target, this, _count * 0.3f);
            }
        }
        public override void ProjectileEnd(Transform target)
        {

        }

        //Cuerpo a cuerpo de fuego
        public override void MeleeActivation(Transform target)
        {
            
        }
        public override void MeleeImpact(Transform target)
        {
            //Queda haciendo dano durante cierto tiempo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.RecieveTemporalDamage(Damage * 0.25f, 2f, 1f);
            }
        }

        //Explosion de fuego
        public override void ExplosionActivation(Transform target)
        {
            
        }
        public override void ExplosionImpact(Transform target)
        {
            //Queda haciendo dano durante cierto tiempo
            if (target.TryGetComponent<EnemyController>(out var cont))
            {
                cont.RecieveTemporalDamage(Damage * 0.25f, 7f, 1f);
            }
        }

        //Potenciador de fuego
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

        public void SetExplosionOnImpact(bool b)
        {
            if (b) _count++;
            else _count--;

            _bombEffect = _count > 0;
        }

    }

}