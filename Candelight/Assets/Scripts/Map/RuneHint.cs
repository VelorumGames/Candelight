using Controls;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class RuneHint : MonoBehaviour
    {
        ESpellInstruction[] _rune;
        [SerializeField] Sprite[] _hintSprites;

        [SerializeField] float _offset;

        private void Start()
        {
            _rune = ChooseRune();
            if (_rune != null) ShowHints();
        }

        ESpellInstruction[] ChooseRune()
        {
            int maxComplexity = (int)FindObjectOfType<MapManager>().CurrentNodeInfo.Biome + 3;

            List<ESpellInstruction[]> runes = new List<ESpellInstruction[]>();
            
            foreach (var chain in ARune.Spells.Keys)
            {
                if (chain.Length <= maxComplexity && !ARune.Spells[chain].IsActivated())
                {
                    runes.Add(chain);
                }
            }
            Debug.Log(runes.Count);
            return runes.Count > 0 ? runes[Random.Range(0, runes.Count)] : null;
        }

        void ShowHints()
        {
            float count = 0f;
            foreach(var instr in _rune)
            {
                GameObject hint = new GameObject("Rune Hint " + count);
                hint.transform.parent = transform;
                SpriteRenderer sprite = hint.AddComponent<SpriteRenderer>();

                sprite.sprite = _hintSprites[(int)instr];

                hint.transform.localPosition = new Vector3((_offset * count - _offset * (_rune.Length - 1 - count++)) * 0.5f, 0f, -0.03f);
            }
        }
    }
}