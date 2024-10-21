using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Projectile : MonoBehaviour
    {
        public event Action OnUpdate;
        public event Action OnImpact;

        private void Update()
        {
            if (OnUpdate != null) OnUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (OnImpact != null) OnImpact();
            }
        }
    }
}