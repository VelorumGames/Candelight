using DG.Tweening;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ManageBookSpell : MonoBehaviour
    {
        Image _rend;
        [SerializeField] Sprite[] _runeSprites;

        private void Awake()
        {
            _rend = GetComponent<Image>();

            _rend.color = new Color(1f, 1f, 1f, 0f);
        }

        public void Show(ARune rune)
        {
            switch(rune.Name)
            {
                case "Melee":
                    _rend.sprite = _runeSprites[0];
                    break;
                case "Projectile":
                    _rend.sprite = _runeSprites[1];
                    break;
                case "Explosion":
                    _rend.sprite = _runeSprites[2];
                    break;
                case "Buff":
                    _rend.sprite = _runeSprites[3];
                    break;
                case "Fire":
                    _rend.sprite = _runeSprites[4];
                    break;
                case "Electric":
                    _rend.sprite = _runeSprites[5];
                    break;
                case "Cosmic":
                    _rend.sprite = _runeSprites[6];
                    break;
                case "Phantom":
                    _rend.sprite = _runeSprites[7];
                    break;
            }

            _rend.DOFade(1f, 0.2f).Play().OnComplete(() => _rend.DOFade(0f, 2f));
            float oScale = GetComponent<RectTransform>().localScale.x;
            GetComponent<RectTransform>().DOScale(oScale * 1.5f, 2f).Play();
        }
    }
}