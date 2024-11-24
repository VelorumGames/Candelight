using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class SacrificadoAnimation : AAnimationController
    {
        Rigidbody _rb;
        PlayerController _cont;

        private new void Awake()
        {
            base.Awake();

            _rb = GetComponentInParent<Rigidbody>();
            _cont = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            Vector3 vel = _rb.velocity.magnitude > 1f ? _rb.velocity.normalized : _rb.velocity;

            Anim.SetFloat("velocity", _rb.velocity.magnitude);
            Anim.SetFloat("xVelocity", vel.x);
            Anim.SetFloat("yVelocity", vel.z);

            Anim.SetFloat("xOrientation", _cont.GetOrientation().x);
            Anim.SetFloat("yOrientation", _cont.GetOrientation().z);
        }
    }
}