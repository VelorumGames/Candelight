using Dialogues;
using Enemy;
using Events;
using Hechizos;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace SpellInteractuable
{
    public class SI_AnchorBarrier : ASpellInteractuable
    {
        [SerializeField] bool _active;
        [SerializeField] int _atCount;
        [SerializeField] int _attackLimit;

        [Space(10)]
        [SerializeField] Sprite[] _brokenBlocks;
        HealthBar _bar;
        [SerializeField] SpriteRenderer _rend;

        AudioSource _audio;

        private void Awake()
        {
            _bar = GetComponentInChildren<HealthBar>();
            _audio = GetComponent<AudioSource>();

            _bar.gameObject.SetActive(false);
        }

        protected override void ApplyInteraction(ASpell spell)
        {
            if (_active)
            {
                _audio.Play();
                if(++_atCount >= _attackLimit)
                {
                    Destroy(_bar.gameObject);
                    Destroy(GetComponent<Collider>());
                    Destroy(_rend);
                }

                if (_atCount > 0) _bar.gameObject.SetActive(true);

                switch(_atCount)
                {
                    case 1:
                        _rend.sprite = _brokenBlocks[0];
                        _bar.ManualUpdateHealthBar(0.75f);
                        break;
                    case 2:
                        _rend.sprite = _brokenBlocks[1];
                        _bar.ManualUpdateHealthBar(0.5f);
                        break;
                    case 3:
                        _rend.sprite = _brokenBlocks[2];
                        _bar.ManualUpdateHealthBar(0.25f);
                        break;
                }
            }
        }

        public bool Activate(bool b) => _active = b;
    }
}