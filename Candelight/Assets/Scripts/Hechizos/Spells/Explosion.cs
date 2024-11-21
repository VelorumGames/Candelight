using DG.Tweening;
using Enemy;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class Explosion : ASpell
    {
        public event Action<Transform> OnImpact;
        public Transform Target;
        public float Damage;
        [SerializeField] float _lifeSpan;

        [SerializeField] MeshRenderer _body;
        [SerializeField] MeshRenderer _secondaryEffect;
        [SerializeField] MeshRenderer _tertiaryEffect;

        [SerializeField] Material[] _materials;

        private void Start()
        {
            //Invoke("Death", _lifeSpan);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Target = other.transform.parent;
                if (OnImpact != null) OnImpact(Target);


                if (Target.TryGetComponent<EnemyController>(out var enemy))
                {
                    enemy.RecieveDamage(Damage);
                } 
                else if (other.transform.TryGetComponent(out enemy))
                {
                    enemy.RecieveDamage(Damage);
                }
            }
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

            switch (runes[0].Name)
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
            
            _body.sharedMaterial.DOVector(new Vector2(0.99f, 1f), Shader.PropertyToID("_Opacity"), _lifeSpan).Play().OnComplete(() =>
            {
                _body.sharedMaterial.SetVector("_Opacity", new Vector2(-1.4f, 1f));
                Death();
            });
            _body.sharedMaterial.DOFloat(-0.5f, Shader.PropertyToID("_SecColorPresence"), _lifeSpan).Play().OnComplete(() =>
            {
                _body.sharedMaterial.SetFloat("_SecColorPresence", 0.01f);
            });

            Vector3 oScale = _body.transform.localScale;
            _body.transform.DOScale(oScale * 1.5f, _lifeSpan).Play().OnComplete(() =>
            {
                _body.transform.localScale = oScale;
            });

            if (runes.Length > 1)
            {
                switch (runes[1].Name)
                {
                    case "Fire":
                        _secondaryEffect.material = _materials[0];
                        break;
                    case "Electric":
                        _secondaryEffect.material = _materials[1];
                        break;
                    case "Cosmic":
                        _secondaryEffect.material = _materials[2];
                        break;
                    case "Phantom":
                        _secondaryEffect.material = _materials[3];
                        break;
                    default:
                        Debug.LogWarning($"ERROR: No se ha procesado bien el nombre de la runa elemental activada ({runes[0].Name}). No se puede mostrar un material adecuado para el proyectil.");
                        break;
                }
                _secondaryEffect.gameObject.SetActive(true);

                _secondaryEffect.sharedMaterial.DOVector(new Vector2(0.99f, 1f), Shader.PropertyToID("_Opacity"), _lifeSpan).Play().OnComplete(() =>
                {
                    _secondaryEffect.sharedMaterial.SetVector("_Opacity", new Vector2(-1.4f, 1f));
                    Death();
                });
                _secondaryEffect.sharedMaterial.DOFloat(-0.5f, Shader.PropertyToID("_SecColorPresence"), _lifeSpan).Play().OnComplete(() =>
                {
                    _secondaryEffect.sharedMaterial.SetFloat("_SecColorPresence", 0.01f);
                });

                Vector3 osScale = _secondaryEffect.transform.localScale;
                _secondaryEffect.transform.DOScale(osScale * 1.5f, _lifeSpan).Play().OnComplete(() =>
                {
                    _secondaryEffect.transform.localScale = osScale;
                });
            }
            
            if (runes.Length > 2)
            {
                switch (runes[2].Name)
                {
                    case "Fire":
                        _tertiaryEffect.material = _materials[0];
                        break;
                    case "Electric":
                        _tertiaryEffect.material = _materials[1];
                        break;
                    case "Cosmic":
                        _tertiaryEffect.material = _materials[2];
                        break;
                    case "Phantom":
                        _tertiaryEffect.material = _materials[3];
                        break;
                    default:
                        Debug.LogWarning($"ERROR: No se ha procesado bien el nombre de la runa elemental activada ({runes[0].Name}). No se puede mostrar un material adecuado para el proyectil.");
                        break;
                }
                _tertiaryEffect.gameObject.SetActive(true);

                _tertiaryEffect.sharedMaterial.DOVector(new Vector2(0.99f, 1f), Shader.PropertyToID("_Opacity"), _lifeSpan).Play().OnComplete(() =>
                {
                    _tertiaryEffect.sharedMaterial.SetVector("_Opacity", new Vector2(-1.4f, 1f));
                    Death();
                });
                _tertiaryEffect.sharedMaterial.DOFloat(-0.5f, Shader.PropertyToID("_SecColorPresence"), _lifeSpan).Play().OnComplete(() =>
                {
                    _tertiaryEffect.sharedMaterial.SetFloat("_SecColorPresence", 0.01f);
                });

                Vector3 otScale = _tertiaryEffect.transform.localScale;
                _tertiaryEffect.transform.DOScale(otScale * 1.5f, _lifeSpan).Play().OnComplete(() =>
                {
                    _tertiaryEffect.transform.localScale = otScale;
                });
            }
        }

        public void Death() => Destroy(gameObject);
    }
}
