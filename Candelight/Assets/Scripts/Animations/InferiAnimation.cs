using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class InferiAnimation : AAnimationController
    {
        Rigidbody _rb;

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

        public void ChangeToAttack() => Anim.SetTrigger("Attack");
    }
}
