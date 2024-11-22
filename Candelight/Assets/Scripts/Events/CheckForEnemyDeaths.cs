using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Dialogues;

namespace Events
{
    public class CheckForEnemyDeaths : MonoBehaviour
    {
        [SerializeField] DialogueAgent _npc;
        [SerializeField] Dialogue _completedDialogue;
        [SerializeField] EnemyController[] _enemies;
        [SerializeField] int _maxHits;
        int m_hits;
        int _npcHits
        {
            get => m_hits;
            set
            {
                m_hits = value;
                if (m_hits >= _maxHits) FailEvent();
            }
        }
        int _count = 0;

        private void OnEnable()
        {
            foreach (var en in _enemies) en.OnDeath += RegisterEnemyDeath;
        }

        void RegisterEnemyDeath(AController enemy)
        {
            enemy.OnDeath -= RegisterEnemyDeath;

            if (++_count >= _enemies.Length)
            {
                FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Completed);
                _npc.ChangeDialogue(_completedDialogue);
            }
        }

        void FailEvent()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Failed);
        }

        private void OnDisable()
        {
            foreach (var en in _enemies) if (en != null) en.OnDeath -= RegisterEnemyDeath;
        }
    }
}
