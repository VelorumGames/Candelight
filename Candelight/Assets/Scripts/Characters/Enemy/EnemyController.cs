using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Enemy
{
    public class EnemyController : AController
    {
        public EnemyInfo Info;
        PlayerController _player;

        public GameObject Fragment;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            Orientation = transform.forward;
        }

        private new void Start()
        {
            _player = FindObjectOfType<PlayerController>();

            MaxHP = Info.BaseHP;
            gameObject.name = $"Enemigo {Info.name}";

            base.Start();

            StartCoroutine(DebugAI());
        }

        private void OnEnable()
        {
            OnDeath += SpawnFragments;
        }

        public void SpawnFragments(AController _)
        {
            int num = Random.Range(Info.MinFragments, Info.MaxFragments);
            Debug.Log($"{gameObject.name} suelta {num} fragmentos");
            for(int i = 0; i < num; i++)
            {
                GameObject fragment = Instantiate(Fragment);
                fragment.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }
        }

        public override void RecieveDamage(float damage)
        {
            Debug.Log($"Enemigo {gameObject.name} recibe {damage} de dano");

            CurrentHP -= damage;
        }

        public void OnAttack()
        {
            if (CanMove)
            {
                Debug.Log($"Enemigo {gameObject.name} ataca con {Info.BaseDamage} de ataque");
                Collider[] objs = Physics.OverlapSphere(transform.position, 2f);
                foreach (var col in objs)
                {
                    //Debug.Log("Controlador encontrado: " + col.gameObject.name);
                    if (col.TryGetComponent<AController>(out var cont) && cont != this)
                    {
                        cont.RecieveDamage(Info.BaseDamage);
                    }
                }
            }
        }

        IEnumerator DebugAI()
        {
            Debug.Log("Se inicia IA de prueba");
            while (true)
            {
                Vector3 target = transform.position + new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
                //Debug.Log("Se encuentra nuevo target: " + target);
                if (Random.value < 0.5f) yield return StartCoroutine(MoveTowards(target, 5f));
                else
                {
                    if (Vector3.Distance(transform.position, _player.transform.position) < 5f)
                    {
                        Debug.Log("Enemigo va a atacar");
                        yield return StartCoroutine(MoveTowards(_player.transform));
                        OnAttack();
                        yield return new WaitForSeconds(1f);
                    }
                }
            }
        }

        private void OnDisable()
        {
            OnDeath -= SpawnFragments;
        }
    }
}