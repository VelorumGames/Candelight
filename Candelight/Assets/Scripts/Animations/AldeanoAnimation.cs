using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AldeanoAnimation : AAnimationController
    {
        Rigidbody _rb;
        public bool Active;

        private new void Awake()
        {
            base.Awake();

            _rb = GetComponentInParent<Rigidbody>();
        }

        private void Update()
        {
            Anim.SetFloat("xVelocity", _rb.velocity.normalized.x);
            Anim.SetFloat("yVelocity", _rb.velocity.normalized.z);
            Anim.SetBool("Active", Active);
        }

        public void ChangeToDown() => Anim.SetTrigger("Down");
        public void ChangeToUp() => Anim.SetTrigger("Up");
        public void ChangeToRight() => Anim.SetTrigger("Right");
        public void ChangeToLeft() => Anim.SetTrigger("Left");
    }
}