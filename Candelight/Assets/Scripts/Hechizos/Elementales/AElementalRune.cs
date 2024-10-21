using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public abstract class AElementalRune : ARune
    {
        Action[] _actions = new Action[8]; //Porque son 8 funciones
        public AElementalRune(Mage m) : base(m, 2, 1f)
        {
            _actions[0] = ProjectileStart;
            _actions[1] = ProjectileUpdate;
            _actions[2] = ProjectileImpact;
            _actions[3] = ProjectileEnd;
            _actions[4] = MeleeActivation;
            _actions[5] = ExplosionActivation;
            _actions[6] = ExplosionImpact;
            _actions[7] = BuffActivation;
        }

        public Action[] GetActions()
        {
            return _actions;
        }

        //Proyectil
        public abstract void ProjectileStart();
        public abstract void ProjectileUpdate();
        public abstract void ProjectileImpact();
        public abstract void ProjectileEnd();

        //Cuerpo a cuerpo
        public abstract void MeleeActivation();

        //Explosion
        public abstract void ExplosionActivation();
        public abstract void ExplosionImpact();

        //Potenciador
        public abstract void BuffActivation();



    }

}