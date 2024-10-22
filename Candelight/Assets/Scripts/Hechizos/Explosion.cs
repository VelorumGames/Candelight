using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Explosion : MonoBehaviour
    {
        public event Action<Transform> OnImpact;
        public Transform Target;
        public float Damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (OnImpact != null) OnImpact(Target);
                if (other.TryGetComponent<EnemyController>(out var enemy))
                {
                    enemy.RecieveDamage(Damage);
                }
            }
        }
    }
}
