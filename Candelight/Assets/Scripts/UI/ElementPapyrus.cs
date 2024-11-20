using Controls;
using DG.Tweening;
using Hechizos;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ElementPapyrus : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI[] _runeTexts;
        [SerializeField] Image[] _runeImages;
        [SerializeField] Sprite[] _runeSprites;
        InputManager _input;
        Mage _mage;

        float _oPos;

        Tween _show;
        Tween _hide;

        private void Awake()
        {
            _input = FindObjectOfType<InputManager>();
            _mage = FindObjectOfType<Mage>();

            _oPos = GetComponent<RectTransform>().position.y;

            DOTween.defaultAutoPlay = AutoPlay.None;
            _show = GetComponent<RectTransform>().DOLocalMoveY(-140f, 0.4f).SetAutoKill(false);
            _hide = GetComponent<RectTransform>().DOLocalMoveY(_oPos, 0.4f).SetAutoKill(false);
            DOTween.defaultAutoPlay = AutoPlay.All;
        }

        private void Start()
        {
            ResetRunes();

            if (_mage != null) LoadRunes(null);
        }

        private void OnEnable()
        {
            if (_input != null )
            {
                _input.OnStartElementMode += Show;
                _input.OnExitElementMode += Hide;
                _mage.OnNewRuneActivation += LoadRunes;
            }
        }

        void Show()
        {
            _hide.Pause();

            _show.Restart();
            _show.Play();
        }

        void Hide()
        {
            _show.Pause();

            _hide.Restart();
            _hide.Play();
        }

        void LoadRunes(ARune rune)
        {
            int i = 0;
            foreach(var spell in ARune.Spells.Values)
            {
                if (spell.IsActivated() && spell is AElementalRune)
                {
                    if (i < _runeTexts.Length)
                    {
                        switch(spell.Name)
                        {
                            case "Fire":
                                _runeImages[i].sprite = _runeSprites[0];
                                break;
                            case "Electric":
                                _runeImages[i].sprite = _runeSprites[1];
                                break;
                            case "Cosmic":
                                _runeImages[i].sprite = _runeSprites[2];
                                break;
                            case "Phantom":
                                _runeImages[i].sprite = _runeSprites[3];
                                break;
                        }
                        _runeTexts[i++].text = $"{spell.GetInstructionsToArrows()}";
                    }
                }
            }
        }

        void ResetRunes()
        {
            foreach (var r in _runeImages) r.color = new Color(r.color.r, r.color.g, r.color.b, 0f);
            foreach (var t in _runeTexts) t.text = "";
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.OnStartElementMode -= Show;
                _input.OnExitElementMode -= Hide;
                _mage.OnNewRuneActivation -= LoadRunes;
            }
        }
    }
}