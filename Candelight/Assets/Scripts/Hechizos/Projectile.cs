using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Projectile : MonoBehaviour
    {
        public event Action<Transform> OnUpdate;
        public event Action<Transform> OnImpact;
        public event Action<Transform> OnEnd;
        public float Damage;
        public Transform Target;

        private void Update()
        {
            if (OnUpdate != null) OnUpdate(Target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (OnImpact != null) OnImpact(Target);
                EnemyController enemy = other.GetComponentInParent<EnemyController>();
                if (enemy)
                {
                    enemy.RecieveDamage(Damage);
                }
            }
        }

        private void OnDestroy()
        {
            if (OnEnd != null) OnEnd(Target);
        }
    }
}