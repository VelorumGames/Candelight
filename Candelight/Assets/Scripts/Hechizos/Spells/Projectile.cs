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

        Rigidbody _rb;

        [SerializeField] float _lifeSpan;
        [SerializeField] TextMeshPro _name;
        [SerializeField] MeshRenderer _body;
        [SerializeField] MeshRenderer _secondaryEffect;
        [SerializeField] MeshRenderer _tertiaryEffect;

        [SerializeField] Material[] _materials;
        [SerializeField] Material[] _secMaterials;
        [SerializeField] TrailRenderer[] _trails;

        public bool AffectsPlayer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private new void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(TimedReset());
        }

        private void Update()
        {
            //Debug.Log("Vel: " + GetComponent<Rigidbody>().velocity);

            if (OnUpdate != null) OnUpdate(Target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!AffectsPlayer) 
            {
                
                if (other.CompareTag("Enemy"))
                {
                   
                    Target = other.transform.parent;

                    if (Target != null && Target.TryGetComponent<EnemyController>(out var enemy))
                    {
                        enemy.RecieveDamage(Damage);
                    }
                    else if (other.transform.TryGetComponent(out enemy))
                    {
                        enemy.RecieveDamage(Damage);
                    }

                    if (OnImpact != null) OnImpact(Target);
                }
            } 
            else
            {
                if(other.CompareTag("Player"))
                {
                    Target = other.transform;

                    if (Target.TryGetComponent<PlayerController>(out var player))
                    {
                        player.RecieveDamage(Damage);
                        gameObject.SetActive(false);
                    }

                    if (OnImpact != null) OnImpact(Target);
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
            Target = null;

            ResetTrail();
            ResetType();
            StopAllCoroutines();
        }

        IEnumerator TimedReset()
        {
            yield return new WaitForSeconds(_lifeSpan);
            gameObject.SetActive(false);
        }

        public void RegisterTypes(AElementalRune[] runes)
        {
            //Chequeo de que se hayan pasado mal los datos
            if (runes.Length == 2 && runes[0].Name == runes[1].Name)
            {
                AElementalRune oldRune = runes[0];
                runes = new AElementalRune[1];
                runes[0] = oldRune;
            }

            switch(runes[0].Name)
            {
                case "Fire":
                    _body.material = _materials[0];
                    break;
                case "Electric":
                    _body.material = _materials[1];
                    break;
                case "Cosmic":
                    _body.material = _materials[2];
                    break;
                case "Phantom":
                    _body.material = _materials[3];
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
                        _secondaryEffect.material = _secMaterials[0];
                        break;
                    case "Electric":
                        _secondaryEffect.material = _secMaterials[1];
                        break;
                    case "Cosmic":
                        _secondaryEffect.material = _secMaterials[2];
                        break;
                    case "Phantom":
                        _secondaryEffect.material = _secMaterials[3];
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
                        _tertiaryEffect.material = _secMaterials[0];
                        break;
                    case "Electric":
                        _tertiaryEffect.material = _secMaterials[1];
                        break;
                    case "Cosmic":
                        _tertiaryEffect.material = _secMaterials[2];
                        break;
                    case "Phantom":
                        _tertiaryEffect.material = _secMaterials[3];
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