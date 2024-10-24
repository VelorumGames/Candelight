using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Melee : MonoBehaviour
    {
        public event Action<Transform> OnImpact;

        public Transform Target;
        public float Damage;

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
    }
}
