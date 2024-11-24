using Controls;
using DG.Tweening;
using Hechizos;
using Hechizos.DeForma;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShapePapyrus : MonoBehaviour
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
            if (_input != null)
            {
                _input.OnStartShapeMode += Show;
                _input.OnExitShapeMode += Hide;
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

        void ResetRunes()
        {
            foreach (var r in _runeImages) r.color = new Color(r.color.r, r.color.g, r.color.b, 0f);
            foreach (var t in _runeTexts) t.text = "";
        }

        void LoadRunes(ARune rune)
        {
            int i = 0;
            foreach (var spell in ARune.Spells.Values)
            {
                //Debug.Log($"Para {spell.Name} tenemos: {spell.IsActivated()} && {spell is AShapeRune}");
                if (spell.IsActivated() && spell is AShapeRune)
                {
                    if (i < _runeTexts.Length)
                    {
                        switch (spell.Name)
                        {
                            case "Melee":
                                _runeImages[i].sprite = _runeSprites[0];
                                break;
                            case "Projectile":
                                _runeImages[i].sprite = _runeSprites[1];
                                break;
                            case "Explosion":
                                _runeImages[i].sprite = _runeSprites[2];
                                break;
                            case "Buff":
                                _runeImages[i].sprite = _runeSprites[3];
                                break;
                        }
                        _runeImages[i].color = new Color(_runeImages[i].color.r, _runeImages[i].color.g, _runeImages[i].color.b, 1f);
                        _runeTexts[i++].text = $"{spell.GetInstructionsToArrows()}";
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.OnStartShapeMode -= Show;
                _input.OnExitShapeMode -= Hide;
                _mage.OnNewRuneActivation -= LoadRunes;
            }
        }
    }
}