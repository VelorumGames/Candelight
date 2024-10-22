using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Melee : MonoBehaviour
    {
        public Transform Target;
        public float Damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent<EnemyController>(out var enemy))
                {
                    enemy.RecieveDamage(Damage);
                }
            }
        }
    }
}
