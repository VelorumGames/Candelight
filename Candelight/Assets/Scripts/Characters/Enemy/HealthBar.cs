using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class HealthBar : MonoBehaviour
    {
        EnemyController _enemy;
        [SerializeField] Transform _mask;
        [SerializeField] TextMeshPro _owlText;

        float _oPos;
        float _endPos = 0.342f;

        private void Awake()
        {
            _enemy = transform.parent.GetComponent<EnemyController>();

            _oPos = _mask.localPosition.x;
        }

        private void OnEnable()
        {
            if (_enemy) _enemy.OnDamage += UpdateHealthBar;
        }

        void UpdateHealthBar(float dam, float rem)
        {
            _mask.localPosition = new Vector3(Mathf.Lerp(_endPos, _oPos, rem), _mask.localPosition.y, _mask.localPosition.z);
            if (GameSettings.Owl) _owlText.text = $"HP: {_enemy.CurrentHP}/{_enemy.MaxHP}";
        }

        public void ManualUpdateHealthBar(float rem)
        {
            _mask.localPosition = new Vector3(Mathf.Lerp(_endPos, _oPos, rem), _mask.localPosition.y, _mask.localPosition.z);
            if (GameSettings.Owl) _owlText.text = $"HP: {_enemy.CurrentHP}/{_enemy.MaxHP}";
        }

        private void OnDisable()
        {
            if (_enemy) _enemy.OnDamage -= UpdateHealthBar;
        }
    }
}