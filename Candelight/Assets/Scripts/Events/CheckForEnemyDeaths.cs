using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Dialogues;
using Map;

namespace Events
{
    public class CheckForEnemyDeaths : MonoBehaviour
    {
        [SerializeField] DialogueAgent _npc;
        AutomaticHealthBar _health;
        [SerializeField] Dialogue _completedDialogue;
        [SerializeField] EnemyController[] _enemies;
        int _count = 0;

        private void Awake()
        {
            _health = _npc.GetComponentInChildren<AutomaticHealthBar>();
        }

        private void OnEnable()
        {
            foreach (var en in _enemies) en.OnDeath += RegisterEnemyDeath;
            _health.OnHealthBarCompletion += FailEvent;

            //Evento -> SP -> Container -> Room
            transform.parent.parent.parent.GetComponent<ARoom>().OnPlayerEnter += _health.StartCountdown;
        }

        void RegisterEnemyDeath(AController enemy)
        {
            enemy.OnDeath -= RegisterEnemyDeath;

            if (++_count >= _enemies.Length)
            {
                _health.StopCountdown();
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
            _health.OnHealthBarCompletion -= FailEvent;

            transform.parent.parent.parent.GetComponent<ARoom>().OnPlayerEnter -= _health.StartCountdown;
        }
    }
}
