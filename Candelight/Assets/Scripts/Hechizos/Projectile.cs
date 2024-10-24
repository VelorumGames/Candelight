using Enemy;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hechizos
{
    public class Projectile : MonoBehaviour
    {
        public event Action<Transform> OnUpdate;
        public event Action<Transform> OnImpact;
        public event Action<Transform> OnEnd;
        public float Damage;
        public Transform Target;

        float _followSpeed;

        Rigidbody _rb;

        [SerializeField] float _lifeSpan;
        [SerializeField] TextMeshPro _name;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine(TimedReset());
        }

        private void Update()
        {
            if (OnUpdate != null) OnUpdate(Target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Target = other.transform.parent;
                if (OnImpact != null) OnImpact(Target);

                if (Target.TryGetComponent<EnemyController>(out var enemy))
                {
                    enemy.RecieveDamage(Damage);
                }

            }
        }

        public void FollowTarget(Transform _)
        {
            if (Target)
            {
                _rb.AddForce(Time.deltaTime * 10000f * (Target.position - transform.position).normalized, ForceMode.Force);
            }
        }

        public void SetFollowSpeed(float s) => _followSpeed = s;

        private void OnDisable()
        {
            if (OnEnd != null) OnEnd(Target);
            OnUpdate = null;
            Target = null;
            ResetType();
            StopAllCoroutines();
        }

        IEnumerator TimedReset()
        {
            yield return new WaitForSeconds(_lifeSpan);
            gameObject.SetActive(false);
        }

        public void RegisterTypes(AElementalRune[] runes)
        {
            foreach (var r in runes)
            {
                _name.text += $"{r.Name} ";
            }
        }

        void ResetType()
        {
            _name.text = "";
        }
    }
}