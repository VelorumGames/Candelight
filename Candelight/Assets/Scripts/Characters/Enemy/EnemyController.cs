using DG.Tweening;
using Items;
using Player;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        protected AudioSource Audio;

        public GameObject Fragment;
        float _fragDropRate = 0.35f;

        bool _invicible;
        bool _canAttack = true;
        float _iFrameDuration = 0.5f;
        bool _dying;

        public bool LuciernagaPosada;
        [Space(10)]
        public AudioClip[] Idles;
        public AudioClip[] Damages;
        public AudioClip Death;

        protected EnemyState EnState;

        protected void Awake()
        {
            _uiMan = FindObjectOfType<UIManager>();
            Audio = GetComponent<AudioSource>();
            EnState = GetComponentInChildren<EnemyState>();

            _rb = GetComponent<Rigidbody>();
            Orientation = transform.forward;

            Player = FindObjectOfType<PlayerController>();
        }

        protected new void Start()
        {
            MaxHP = Info.BaseHP;
            gameObject.name = $"Enemigo {Info.Name}";

            base.Start();

            StartCoroutine(IdleSound());
        }

        protected void OnEnable()
        {
            OnDeath += SpawnFragments;
            OnDeath += PlayDeathSound;
            OnDeath += FakeDeath;
            OnDamage += _uiMan.EnemyDamageFeedback;
        }

        public void SpawnFragments(AController _)
        {
            Vector3 spawnPos = (Player.transform.position + transform.position) / 2f;
            FindObjectOfType<Inventory>().SpawnFragments(Random.Range(Info.MinFragments, Info.MaxFragments), _fragDropRate * _modifier.FragDropMod, spawnPos);
        }

        void PlayDeathSound(AController _) => Audio.PlayOneShot(Death);

        public override void RecieveDamage(float damage)
        {
            if (!_invicible && !_dying)
            {
                if (LuciernagaPosada)
                {
                    damage *= 2;
                    Debug.Log($"{gameObject.name} recibe el doble de dano porque tiene una luciernaga posada.");
                }

                CurrentHP -= damage;
                ShowDamage(damage);
                CallDamageEvent(damage, CurrentHP / MaxHP);
                Audio.PlayOneShot(Damages[Random.Range(0, Damages.Length)]);

                _invicible = true;
                Invoke("ManageIFrames", _iFrameDuration);
            }
        }

        void FakeDeath(AController _)
        {
            _dying = true;
            _canAttack = false;
        }

        public void RegisterDamageType(string type)
        {
            switch(type)
            {
                case "Fire":
                    EnState.ShowState("Quemadura");
                    break;
                case "Paralize":
                    EnState.ShowState("Paralizado");
                    break;
                case "Slowness":
                    EnState.ShowState("Ralentizado");
                    break;
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

            dam.transform.position = transform.position;
            float target = dam.transform.position.y + 2f;
            dam.transform.DOMoveY(target, 1f).Play();

            damText.text = $"-{damage:F0}";
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
            if (CanMove && _canAttack)
            {
                Collider[] objs = Physics.OverlapSphere(transform.position, 2f);
                foreach (var col in objs)
                {
                    if (col.TryGetComponent<PlayerController>(out var cont) && cont != this)
                    {
                        cont.RecieveDamage(Info.BaseDamage * _modifier.DamageMod);
                    }
                }
            }
        }

        IEnumerator DebugAI()
        {
            while (true)
            {
                Vector3 target = transform.position + new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
                if (Random.value < 0.5f) yield return StartCoroutine(MoveTowards(target, 5f));
                else
                {
                    if (Vector3.Distance(transform.position, Player.transform.position) < 5f)
                    {
                        yield return StartCoroutine(MoveTowards(Player.transform));
                        OnAttack();
                        yield return new WaitForSeconds(1f);
                    }
                }
            }
        }

        IEnumerator IdleSound()
        {
            while (true)
            {
                Audio.PlayOneShot(Idles[Random.Range(0, Idles.Length)]);
                yield return new WaitForSeconds(Random.Range(1f, 6f));
            }
        }

        protected void OnDisable()
        {
            OnDeath -= SpawnFragments;
            OnDeath -= PlayDeathSound;
            OnDeath -= FakeDeath;
            OnDamage -= _uiMan.EnemyDamageFeedback;
        }
    }
}