using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.DeForma
{
    public class ProjectileRune : AShapeRune
    {

        public event Action<Transform> OnStartProjectile;
        public event Action<Transform> OnUpdateProjectile;
        public event Action<Transform> OnImpactProjectile;
        public event Action<Transform> OnEndProjectile;

        public ProjectileRune(Mage m) : base(m)
        {
            Name = "Proyectil";
        }
        public override void LoadElements(Action<Transform>[] actions)
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
            GameObject projGO = MageManager.SpawnProjectile();
            if (OnStartProjectile != null) OnStartProjectile(MageManager.transform);

            Projectile proj = projGO.GetComponent<Projectile>();
            proj.OnUpdate += ProjectileUpdate;
            proj.OnImpact += ProjectileImpact;
            proj.OnEnd += ProjectileEnd;

            float avDam = 0;
            foreach (var el in MageManager.GetActiveElements()) avDam += el.Damage;
            avDam /= MageManager.GetMaxElements();
            proj.Damage = avDam;
        }

        void ProjectileUpdate(Transform target)
        {
            if (OnUpdateProjectile != null) OnUpdateProjectile(target);
        }

        void ProjectileImpact(Transform target)
        {
            if (OnImpactProjectile != null) OnImpactProjectile(target);
        }

        void ProjectileEnd(Transform target)
        {
            if (OnEndProjectile != null) OnEndProjectile(target);
        }
    }

}