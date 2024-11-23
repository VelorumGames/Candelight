using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class SombraAnimation : AAnimationController
    {
        Vector3 _orToPlayer;

        PlayerController _player;



        private new void Awake()
        {
            base.Awake();
            _player = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            _orToPlayer = (_player.transform.position - transform.position).normalized;

            Anim.SetFloat("xOrientation", -_orToPlayer.x);
            Anim.SetFloat("yOrientation", -_orToPlayer.z);
        }

        public void ChangeToIdle()
        {
            if (Anim != null) Anim.SetTrigger("Idle");
        }
        public void ChangeToDivide()
        {
            if (Anim != null) Anim.SetTrigger("Divide");
        }
        public void ChangeToShoot()
        {
            if (Anim != null) Anim.SetTrigger("Shoot");
        }
    }
}