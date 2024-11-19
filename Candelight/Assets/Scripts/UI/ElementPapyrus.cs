using Controls;
using DG.Tweening;
using Hechizos;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ElementPapyrus : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI[] _runeTexts;
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
                    if (i < _runeTexts.Length) _runeTexts[i++].text = $"{spell.Name}: {spell.GetInstructionsToString()}";
                }
            }
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