using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class LuciernagaAnimation : AAnimationController
    {
        bool _enemyFound;
        bool _orbitingEnemy;

        private void Update()
        {
            if (!_enemyFound && Time.frameCount % 60 == 0)
            {
                Anim.SetInteger("FlyId", Random.Range(0, 5));
            }
        }

        public void SetEnemyFound(bool b)
        {
            if (b != _enemyFound)
            {
                _enemyFound = b;
                Anim.SetBool("Enemy", b);
            }
        }

        public void GetToEnemy(bool b)
        {
            if (b != _orbitingEnemy)
            {
                _orbitingEnemy = b;
                Anim.SetBool("OrbitingEnemy", b);

                if (!b)
                {
                    SetEnemyFound(false);
                }
            }
        }
    }
}
