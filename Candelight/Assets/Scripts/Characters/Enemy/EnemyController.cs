using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Enemy
{
    public class EnemyController : AController
    {
        public EnemyInfo Info;

        private void Start()
        {
            MaxHP = Info.BaseHP;
            CurrentHP = MaxHP;

            StartCoroutine(DebugAI());
        }

        public void OnAttack()
        {
            Debug.Log($"Enemigo {gameObject.name} ataca con {Info.BaseDamage} de ataque");
        }

        IEnumerator DebugAI()
        {
            Debug.Log("Se inicia IA de prueba");
            while (true)
            {
                Vector3 target = transform.position + new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
                Debug.Log("Se encuentra nuevo target: " + target);
                yield return StartCoroutine(MoveTowards(target, 5f));
            }
        }
    }
}