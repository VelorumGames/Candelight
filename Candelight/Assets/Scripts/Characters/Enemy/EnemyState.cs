using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyState : MonoBehaviour
    {
        [SerializeField] Sprite[] _sprites;
        SpriteRenderer _rend;

        private void Awake()
        {
            _rend = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ResetState();
        }

        public void ShowState(string s)
        {
            StopAllCoroutines();
            StartCoroutine(ManageState(s));
        }

        IEnumerator ManageState(string stateName)
        {
            switch(stateName)
            {
                case "CobreCalmado":
                    _rend.sprite = _sprites[0];
                    break;
                case "CobreEnfadado":
                    _rend.sprite = _sprites[1];
                    break;
                case "MurcConfuso":
                    _rend.sprite = _sprites[2];
                    break;
                case "MurcDetectado":
                    _rend.sprite = _sprites[3];
                    break;
                case "SombraEnfadada":
                    _rend.sprite = _sprites[3];
                    break;
                case "Quemadura":
                    _rend.sprite = _sprites[4];
                    break;
                case "Paralizado":
                    _rend.sprite = _sprites[5];
                    break;
                case "Ralentizado":
                    _rend.sprite = _sprites[6];
                    break;
                case "InferiLider":
                    _rend.sprite = _sprites[7];
                    break;
            }

            yield return new WaitForSeconds(4f);

            ResetState();
        }

        public void ResetState() => _rend.color = new Color(1f, 1f, 1f, 0f);
    }
}