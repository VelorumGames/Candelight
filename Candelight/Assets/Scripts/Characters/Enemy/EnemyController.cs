using DG.Tweening;
using Items;
using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using World;

namespace Enemy
{
    public abstract class EnemyController : AController
    {
        public EnemyInfo Info;
        [SerializeField] EnemyModifiers _modifier;
        [SerializeField] GameObject _damagePrefab;
        protected PlayerController Player;
        UIManager _uiMan;

        public GameObject Fragment;
        float _fragDropRate = 0.5f;

        bool _invicible;
        float _iFrameDuration = 0.5f;

        protected void Awake()
        {
            _uiMan = FindObjectOfType<UIManager>();

            _rb = GetComponent<Rigidbody>();
            Orientation = transform.forward;

            Player = FindObjectOfType<PlayerController>();
        }

        protected new void Start()
        {
            MaxHP = Info.BaseHP;
            gameObject.name = $"Enemigo {Info.name}";

            base.Start();

            StartCoroutine(DebugAI());
        }

        private void OnEnable()
        {
            OnDeath += SpawnFragments;
            OnDamage += _uiMan.EnemyDamageFeedback;
        }

        public void SpawnFragments(AController _)
        {
            FindObjectOfType<Inventory>().SpawnFragments(Random.Range(Info.MinFragments, Info.MaxFragments), _fragDropRate * _modifier.FragDropMod, transform);

            //int num = Random.Range(Info.MinFragments, Info.MaxFragments);
            //Debug.Log($"{gameObject.name} suelta {num} fragmentos");
            //for(int i = 0; i < num; i++)
            //{
            //    if (Random.value < _fragDropRate * _modifier.FragDropMod)
            //    {
            //        GameObject fragment = Instantiate(Fragment);
            //        fragment.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            //    }
            //}
        }

        public override void RecieveDamage(float damage)
        {
            if (!_invicible)
            {
                //Debug.Log($"Enemigo {gameObject.name} recibe {damage} de dano");

                CurrentHP -= damage;
                ShowDamage(damage);
                CallDamageEvent(damage, CurrentHP / MaxHP);

                _invicible = true;
                Invoke("ManageIFrames", _iFrameDuration);
                
            }
        }

        public void ManageIFrames()
        {
            _invicible = false;
        }

        void ShowDamage(float damage)
        {
            GameObject dam = Instantiate(_damagePrefab);
            TextMeshPro damText = dam.GetComponent<TextMeshPro>();

            //GameObject damGO = Instantiate(dam);
            dam.transform.position = transform.position;
            float target = dam.transform.position.y + 2f;
            dam.transform.DOMoveY(target, 1f).Play();

            damText.text = $"-{damage}";
            damText.DOFade(0f, 1f).Play().OnComplete(() => Destroy(dam));
            dam.transform.DOLocalMoveY(5f, 1f).Play();
        }

        public new void OnMove(Vector2 direction)
        {
            if (CanMove)
            {
                if (!_rb) _rb = GetComponent<Rigidbody>();
                Vector3 force = Time.deltaTime * 100f * _speed * _modifier.SpeedMod * new Vector3(direction.x, 0f, direction.y);
                _rb.AddForce(force, ForceMode.Force);
            }
        }

        public void OnAttack()
        {
            if (CanMove)
            {
                //Debug.Log($"Enemigo {gameObject.name} ataca con {Info.BaseDamage} (+ {Info.BaseDamage * (_modifier.DamageMod - 1)}) de ataque");
                Collider[] objs = Physics.OverlapSphere(transform.position, 2f);
                foreach (var col in objs)
                {
                    //Debug.Log("Controlador encontrado: " + col.gameObject.name);
                    if (col.TryGetComponent<AController>(out var cont) && cont != this)
                    {
                        cont.RecieveDamage(Info.BaseDamage * _modifier.DamageMod);
                    }
                }
            }
        }

        IEnumerator DebugAI()
        {
            //Debug.Log("Se inicia IA de prueba");
            while (true)
            {
                Vector3 target = transform.position + new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
                //Debug.Log("Se encuentra nuevo target: " + target);
                if (Random.value < 0.5f) yield return StartCoroutine(MoveTowards(target, 5f));
                else
                {
                    if (Vector3.Distance(transform.position, Player.transform.position) < 5f)
                    {
                        //Debug.Log("Enemigo va a atacar");
                        yield return StartCoroutine(MoveTowards(Player.transform));
                        OnAttack();
                        yield return new WaitForSeconds(1f);
                    }
                }
            }
        }

        private void OnDisable()
        {
            OnDeath -= SpawnFragments;
            OnDamage -= _uiMan.EnemyDamageFeedback;
        }
    }
}