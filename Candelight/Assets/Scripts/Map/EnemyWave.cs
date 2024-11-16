using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class EnemyWave : MonoBehaviour
    {
        ChallengeRoom _challenge;
        int _numEnemies;
        int _count;

        private void Awake()
        {
            _challenge = FindObjectOfType<ChallengeRoom>();
        }

        private void Start()
        {
            _numEnemies = FindChildren();
        }

        int FindChildren()
        {
            EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
            foreach (var e in enemies) e.OnDeath += RegisterDeath;
            return enemies.Length;
        }

        void RegisterDeath(AController sender)
        {
            sender.OnDeath -= RegisterDeath;
            _count++;

            if(_count >= _numEnemies)
            {
                _challenge.SpawnNextWave();
            }
        }
    }
}
