using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class ProjectileRune : AShapeRune
    {

        public event Action OnStartProjectile;
        public event Action OnUpdateProjectile;
        public event Action OnImpactProjectile;
        public event Action OnEndProjectile;

        public ProjectileRune(Mage m) : base(m)
        {
            Name = "Proyectil";
        }
        public override void LoadElements(Action[] actions)
        {
            OnStartProjectile += actions[0];
            OnUpdateProjectile += actions[1];
            OnImpactProjectile += actions[2];
            OnEndProjectile += actions[3];
        }

        public override void ResetElements()
        {
            OnStartProjectile = null;
            OnUpdateProjectile = null;
            OnImpactProjectile = null;
            OnEndProjectile = null;
        }

        public override void ThrowSpell()
        {
            GameObject proj = MageManager.SpawnProjectile();
            if (OnStartProjectile != null) OnStartProjectile();
            proj.GetComponent<Projectile>().OnUpdate += ProjectileUpdate;
            proj.GetComponent<Projectile>().OnImpact += ProjectileImpact;
        }

        void ProjectileUpdate()
        {
            if (OnUpdateProjectile != null) OnUpdateProjectile();
        }

        void ProjectileImpact()
        {
            if (OnImpactProjectile != null) OnImpactProjectile();
        }
    }

}