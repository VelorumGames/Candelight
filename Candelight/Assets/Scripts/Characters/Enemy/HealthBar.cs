using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class HealthBar : MonoBehaviour
    {
        EnemyController _enemy;
        [SerializeField] Transform _mask;

        float _oPos;
        float _endPos = 0.342f;

        private void Awake()
        {
            _enemy = transform.parent.GetComponent<EnemyController>();

            _oPos = _mask.localPosition.x;
        }

        private void OnEnable()
        {
            _enemy.OnDamage += UpdateHealthBar;
        }

        void UpdateHealthBar(float dam, float rem)
        {
            _mask.localPosition = new Vector3(Mathf.Lerp(_endPos, _oPos, rem), _mask.localPosition.y, _mask.localPosition.z);
        }

        private void OnDisable()
        {
            _enemy.OnDamage -= UpdateHealthBar;
        }
    }
}