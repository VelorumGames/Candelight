using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class TemerianDollItem : AItem
    {
        [SerializeField] EnemyModifiers _mod;
        [SerializeField] float _extraEnemySpeed = 1.1f;
        [SerializeField] float _extraEnemyDamage = 1.1f;
        [SerializeField] float _extraFragmentRate = 1.1f;

        protected override void ApplyProperty()
        {
            // Los enemigos se mueven un 10% mas rapido y hacen un 10% mas de dano, pero aumenta ligeramente la probabilidad de que suelten cristales
            _mod.SpeedMod *= _extraEnemySpeed;
            _mod.DamageMod *= _extraEnemyDamage;
            _mod.FragDropMod *= _extraFragmentRate;
        }

        protected override void ResetProperty()
        {
            _mod.SpeedMod /= _extraEnemySpeed;
            _mod.DamageMod /= _extraEnemyDamage;
            _mod.FragDropMod /= _extraFragmentRate;
        }
    }
}