using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class MurcielagoAnimation : AAnimationController
    {
        Rigidbody _rb;

        private new void Awake()
        {
            base.Awake();
            _rb = GetComponentInParent<Rigidbody>();
        }

        private void Update()
        {
            Anim.SetFloat("xVelocity", _rb.velocity.x);
            Anim.SetFloat("yVelocity", _rb.velocity.z);
        }

        public void ChangeToDown() => Anim.SetTrigger("Down");
        public void ChangeToUp() => Anim.SetTrigger("Up");
        public void ChangeToLeft() => Anim.SetTrigger("Left");
        public void ChangeToRight() => Anim.SetTrigger("Right");
        public void ChangeToAttack() => Anim.SetTrigger("Attack");
    }
}
