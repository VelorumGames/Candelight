using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public abstract class AElementalRune : ARune
    {
        protected float Damage;
        protected float DamageFactor = 1f;
        Action<Transform>[] _actions = new Action<Transform>[9]; //Porque son 9 funciones

        public AElementalRune(Mage m) : base(m, 2, 1f)
        {
            _actions[0] = ProjectileStart;
            _actions[1] = ProjectileUpdate;
            _actions[2] = ProjectileImpact;
            _actions[3] = ProjectileEnd;
            _actions[4] = MeleeActivation;
            _actions[5] = MeleeImpact;
            _actions[6] = ExplosionActivation;
            _actions[7] = ExplosionImpact;
            _actions[8] = BuffActivation;
        }

        public Action<Transform>[] GetActions()
        {
            return _actions;
        }

        //Proyectil
        public abstract void ProjectileStart(Transform target);
        public abstract void ProjectileUpdate(Transform target);
        public abstract void ProjectileImpact(Transform target);
        public abstract void ProjectileEnd(Transform target);

        //Cuerpo a cuerpo
        public abstract void MeleeActivation(Transform target);
        public abstract void MeleeImpact(Transform target);

        //Explosion
        public abstract void ExplosionActivation(Transform target);
        public abstract void ExplosionImpact(Transform target);

        //Potenciador
        public abstract void BuffActivation(Transform target);

        public float GetDamage() => Damage * DamageFactor;
        public float SetDamageFactor(float factor) => DamageFactor = factor;

    }

}