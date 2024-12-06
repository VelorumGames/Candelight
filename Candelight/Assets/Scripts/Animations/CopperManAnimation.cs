using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class CopperManAnimation : AAnimationController
    {
        Rigidbody _rb;

        Vector3 _velocity;

        private new void Awake()
        {
            base.Awake();

            _rb = GetComponentInParent<Rigidbody>();
        }

        private void Update()
        {
            Anim.SetFloat("xVelocity", _rb.velocity.normalized.x);
            Anim.SetFloat("yVelocity", _rb.velocity.normalized.z);
        }

        public void ChangeToMelee() => Anim.SetTrigger("Melee");
        public void ChangeToShoot() => Anim.SetTrigger("Shoot");
    }
}