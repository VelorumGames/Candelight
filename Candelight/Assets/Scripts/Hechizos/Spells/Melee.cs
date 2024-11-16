using Enemy;
using SpellInteractuable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Melee : ASpell
    {
        public event Action<Transform> OnImpact;

        public Transform Target;
        public float Damage;
        [SerializeField] float _lifeSpan;

        private void Start()
        {
            Invoke("Death", _lifeSpan);

            Collider[] nearCols = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            foreach(var c in nearCols)
            {
                if (c.TryGetComponent(out ASpellInteractuable inter))
                {
                    inter.ForceApplyInteraction(this);
                    break;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Target = other.transform.parent;
                if (OnImpact != null) OnImpact(Target);

                if (Target.TryGetComponent<EnemyController>(out var enemy))
                {
                    enemy.RecieveDamage(Damage);
                }
            }
        }

        public void Death() => Destroy(gameObject);
    }
}
