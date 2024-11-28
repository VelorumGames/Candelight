using Controls;
using Hechizos;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class RuneHint : MonoBehaviour
    {
        ESpellInstruction[] _rune;
        [SerializeField] ParticleSystemRenderer _particles;
        [SerializeField] Material[] _runeMats;
        [SerializeField] Material _runeMaterial;

        [SerializeField] Sprite[] _hintSprites;

        [SerializeField] string _runeToFind;
        [SerializeField] Color _runeColor;

        [SerializeField] Vector2 _offset;

        private void Awake()
        {
            //Debug. Deberia estar desactivado
            //ARune.CreateAllRunes(FindObjectOfType<Mage>());
        }

        private void Start()
        {
            _rune = ChooseRune();
            if (_rune != null)
            {
                ShowHints();
            }
            else gameObject.SetActive(false);
        }

        ESpellInstruction[] ChooseRune()
        {
            if (_runeToFind != "")
            {
                if (ARune.FindSpell(_runeToFind, out var spell))
                {
                    LoadParticles(spell);
                    return spell.GetInstructions();
                }
                else return null;
            }
            else
            {
                int maxComplexity = (int)FindObjectOfType<MapManager>().CurrentNodeInfo.Biome + 3;

                List<ESpellInstruction[]> runes = new List<ESpellInstruction[]>();

                foreach (var chain in ARune.Spells.Keys)
                {
                    if (chain.Length <= maxComplexity && !ARune.Spells[chain].IsActivated())
                    {
                        runes.Add(chain);
                        LoadParticles(ARune.Spells[chain]);
                        break;
                    }
                }

                return runes.Count > 0 ? runes[Random.Range(0, runes.Count)] : null;
            }
        }

        void ShowHints()
        {
            float count = 0f;
            foreach(var instr in _rune)
            {
                GameObject hint = new GameObject("Rune Hint " + count);
                hint.transform.parent = transform;
                SpriteRenderer sprite = hint.AddComponent<SpriteRenderer>();
                sprite.sharedMaterial = _runeMaterial;

                sprite.sprite = _hintSprites[(int)instr];
                sprite.transform.localScale = 0.5f * Vector3.one;
                sprite.color = _runeColor;

                hint.transform.localPosition = new Vector3((_offset.x * count - _offset.x * (_rune.Length - 1 - count++)) * 0.5f, _offset.y, -0.03f);
            }
        }

        void LoadParticles(ARune rune) => _particles.sharedMaterial = _runeMats[rune is AElementalRune ? 0 : 1];
    }
}