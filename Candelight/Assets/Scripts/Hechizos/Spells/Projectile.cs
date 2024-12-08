using Enemy;
using Hechizos.Elementales;
using SpellInteractuable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using Player;
using Comportamientos.Sombra;
using DG.Tweening;

namespace Hechizos
{
    public class Projectile : ASpell
    {
        public event Action<Transform> OnUpdate;
        public event Action<Transform> OnImpact;
        public event Action<Transform> OnEnd;
        public float Damage;
        public Transform Target;

        float _followSpeed;
        float _oScale;

        Rigidbody _rb;

        [SerializeField] float _lifeSpan;
        [SerializeField] TextMeshPro _name;
        [SerializeField] MeshRenderer _body;
        [SerializeField] MeshRenderer _secondaryEffect;
        [SerializeField] MeshRenderer _tertiaryEffect;

        [SerializeField] Material[] _materials;
        [SerializeField] Material[] _secMaterials;
        [SerializeField] TrailRenderer[] _trails;

        public Vector3 Direction;

        bool _impacted;

        private new void Awake()
        {
            base.Awake();

            _rb = GetComponent<Rigidbody>();

            _oScale = transform.localScale.x;
        }

        private new void OnEnable()
        {
            base.OnEnable();

            //RegisterTypes(Elements);

            _impacted = false;
            transform.localScale = _oScale * Vector3.one;

            StartCoroutine(TimedReset());
        }

        private void Update()
        {
            //Debug.Log("Vel: " + GetComponent<Rigidbody>().velocity);

            if (OnUpdate != null) OnUpdate(Target);

            transform.Translate(Direction * Time.deltaTime);
        }

        public void Push(Vector3 dir, float speed)
        {
            Direction = speed * dir;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_impacted)
            {
                if (!AffectsPlayer)
                {
                    if (other.CompareTag("Enemy"))
                    {
                        Target = other.transform.parent;

                        if (Target != null && Target.TryGetComponent<EnemyController>(out var enemy))
                        {
                            enemy.RecieveDamage(Damage);
                            if (OnImpact != null) OnImpact(Target);
                            _impacted = true;
                        }
                        else if (other.transform.TryGetComponent(out enemy))
                        {
                            enemy.RecieveDamage(Damage);
                            if (OnImpact != null) OnImpact(other.transform);
                            _impacted = true;
                        }

                        //if (OnImpact != null) OnImpact(Target != null ? Target : other.transform);
                    }
                }
                else
                {
                    if (other.CompareTag("Player"))
                    {
                        Target = other.transform;

                        if (Target.TryGetComponent<PlayerController>(out var player))
                        {
                            player.RecieveDamage(Damage);
                            gameObject.SetActive(false);

                            if (OnImpact != null) OnImpact(Target);
                            _impacted = true;
                        }
                    }
                }
            }
        }

        public void FollowTarget(Transform _)
        {
            if (Target)
            {
                _rb.AddForce(Time.deltaTime * 10000f * (Target.position - transform.position).normalized, ForceMode.Force);
            }
        }

        public void SetFollowSpeed(float s) => _followSpeed = s;

        private void OnDisable()
        {
            if (OnEnd != null) OnEnd(Target);
            OnUpdate = null;
            OnImpact = null;
            Target = null;

            ResetTrail();
            ResetType();
            StopAllCoroutines();
        }

        IEnumerator TimedReset()
        {
            yield return new WaitForSeconds(_lifeSpan);
            transform.DOScale(0f, 1f).OnComplete(() => gameObject.SetActive(false)).Play();
        }

        protected override void RegisterTypes(AElementalRune[] runes)
        {
            //Chequeo de que se hayan pasado mal los datos
            if (runes.Length == 2 && runes[0].Name == runes[1].Name)
            {
                AElementalRune oldRune = runes[0];
                runes = new AElementalRune[1];
                runes[0] = oldRune;
            }
            else if (runes.Length == 3 && runes[0].Name == runes[1].Name) //En caso de 3 elementos a la vez
            {
                AElementalRune[] oldRunes = runes;
                runes = new AElementalRune[2];
                runes[0] = oldRunes[0];
                runes[1] = oldRunes[2];
            }
            else if (runes.Length == 3 && runes[0].Name == runes[2].Name || runes.Length == 3 && runes[1].Name == runes[2].Name)
            {
                AElementalRune[] oldRunes = runes;
                runes = new AElementalRune[2];
                runes[0] = oldRunes[0];
                runes[1] = oldRunes[1];
            }

            Elements = runes;

            //string s = "SE REGISTRAN ELEMENTOS: ";
            //foreach (var r in runes)
            //{
            //    s += $"{r.Name}, ";
            //}
            //Debug.Log(s);

            switch (runes[0].Name)
            {
                case "Fire":
                    _body.sharedMaterial = _materials[0];
                    break;
                case "Electric":
                    _body.sharedMaterial = _materials[1];
                    break;
                case "Cosmic":
                    _body.sharedMaterial = _materials[2];
                    break;
                case "Phantom":
                    _body.sharedMaterial = _materials[3];
                    break;
                default:
                    Debug.LogWarning($"ERROR: No se ha procesado bien el nombre de la runa elemental activada ({runes[0].Name}). No se puede mostrar un material adecuado para el proyectil.");
                    break;
            }

            if (runes.Length > 1)
            {
                switch (runes[1].Name)
                {
                    case "Fire":
                        _secondaryEffect.sharedMaterial = _secMaterials[0];
                        break;
                    case "Electric":
                        _secondaryEffect.sharedMaterial = _secMaterials[1];
                        break;
                    case "Cosmic":
                        _secondaryEffect.sharedMaterial = _secMaterials[2];
                        break;
                    case "Phantom":
                        _secondaryEffect.sharedMaterial = _secMaterials[3];
                        break;
                    default:
                        Debug.LogWarning($"ERROR: No se ha procesado bien el nombre de la runa elemental activada ({runes[0].Name}). No se puede mostrar un material adecuado para el proyectil.");
                        break;
                }
                _secondaryEffect.gameObject.SetActive(true);
            }

            if (runes.Length > 2)
            {
                switch (runes[2].Name)
                {
                    case "Fire":
                        _tertiaryEffect.sharedMaterial = _secMaterials[0];
                        break;
                    case "Electric":
                        _tertiaryEffect.sharedMaterial = _secMaterials[1];
                        break;
                    case "Cosmic":
                        _tertiaryEffect.sharedMaterial = _secMaterials[2];
                        break;
                    case "Phantom":
                        _tertiaryEffect.sharedMaterial = _secMaterials[3];
                        break;
                    default:
                        Debug.LogWarning($"ERROR: No se ha procesado bien el nombre de la runa elemental activada ({runes[0].Name}). No se puede mostrar un material adecuado para el proyectil.");
                        break;
                }
                _tertiaryEffect.gameObject.SetActive(true);
            }
        }

        void ResetType()
        {
            _name.text = "";
            _secondaryEffect.gameObject.SetActive(false);
            _tertiaryEffect.gameObject.SetActive(false);
        }

        public void ShowTrail(EElements element)
        {
            _trails[(int)element].gameObject.SetActive(true);
        }

        

        public void ResetTrail()
        {
            foreach (var t in _trails)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}