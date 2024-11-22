using Controls;
using DG.Tweening;
using Hechizos;
using Hechizos.DeForma;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowInstructions : MonoBehaviour
    {
        [SerializeField] Sprite[] _instrSprites;
        [SerializeField] Image[] _elementSprites;
        [SerializeField] Image[] _orbSprites;
        [SerializeField] Image[] _imgs;
        [SerializeField] RectTransform _elementContainer;
        [SerializeField] RectTransform[] _masks;
        List<RectTransform> _elementTransforms = new List<RectTransform>();
        int _current;

        bool _activeCoroutine;

        [Space(10)]
        [SerializeField] Image[] _results;
        [SerializeField] Sprite[] _spellResults;

        private void Start()
        {
            ResetSprites();
            ShowElements();
        }

        public void ShowElements()
        {
            ResetElements();
            if (Mage.Instance != null)
            {
                ResetElements();
                _orbSprites[Math.Clamp(Mage.Instance.GetActiveElements().Count - 1, 0, 1)].gameObject.SetActive(true);

                foreach (var activeEl in Mage.Instance.GetActiveElements())
                {
                    switch (activeEl.Name)
                    {
                        case "Fire":
                            _elementSprites[0].gameObject.SetActive(true);
                            _elementTransforms.Add(_elementSprites[0].GetComponent<RectTransform>());
                            break;
                        case "Electric":
                            _elementSprites[1].gameObject.SetActive(true);
                            _elementTransforms.Add(_elementSprites[1].GetComponent<RectTransform>());
                            break;
                        case "Cosmic":
                            _elementSprites[2].gameObject.SetActive(true);
                            _elementTransforms.Add(_elementSprites[2].GetComponent<RectTransform>());
                            break;
                        case "Phantom":
                            _elementSprites[3].gameObject.SetActive(true);
                            _elementTransforms.Add(_elementSprites[3].GetComponent<RectTransform>());
                            break;
                        default:
                            Debug.Log("ERROR: No se ha encontrado ningun sprite con este nombre: " + activeEl.Name);
                            break;
                    }
                }

                switch (_elementTransforms.Count)
                {
                    case 1:
                        _elementTransforms[0].SetParent(_masks[0]);
                        _masks[0].gameObject.SetActive(true);
                        break;
                    case 2:
                        _elementTransforms[0].SetParent(_masks[1]);
                        _elementTransforms[1].SetParent(_masks[2]);
                        _masks[1].gameObject.SetActive(true);
                        _masks[2].gameObject.SetActive(true);
                        break;
                    case 3:
                        _elementTransforms[0].SetParent(_masks[3]);
                        _elementTransforms[1].SetParent(_masks[4]);
                        _elementTransforms[2].SetParent(_masks[5]);
                        _masks[3].gameObject.SetActive(true);
                        _masks[4].gameObject.SetActive(true);
                        _masks[5].gameObject.SetActive(true);
                        break;
                }
            }
        }

        public void ResetElements()
        {
            foreach (var orb in _orbSprites) orb.gameObject.SetActive(false);
            foreach (var el in _elementSprites) el.gameObject.SetActive(false);
            foreach (var m in _masks) m.gameObject.SetActive(false);
            if (_elementTransforms.Count > 0)
            {
                foreach (var el in _elementTransforms) el.SetParent(_elementContainer);
                _elementTransforms.Clear();
            }
        }

        public void ShowInstruction(ESpellInstruction instr)
        {
            if (_activeCoroutine)
            {
                StopAllCoroutines();
                ResetSprites();
            }

            if (_current == 0) ResetSprites();

            _imgs[_current].gameObject.SetActive(true);
            _imgs[_current++].sprite = _instrSprites[(int)instr];

            _current %= _imgs.Length;
        }

        public void ResetSprites()
        {
            //Debug.Log("Reseteo sprites");
            _current = 0;

            foreach (var i in _imgs)
            {
                if (i != null)
                {
                    i.gameObject.SetActive(false);
                    i.color = i.GetComponent<InstructionUI>().GetColor();
                    i.sprite = null;
                }
            }
        }

        public IEnumerator ShowValidInstructions()
        {
            _activeCoroutine = true;
            foreach (var i in _imgs)
            {
                i.color = Color.yellow;
            }
            yield return new WaitForSeconds(1f);
            _activeCoroutine = false;
            ResetSprites();
        }

        public void ShowShapeResult(AShapeRune rune)
        {
            switch (rune.Name)
            {
                case "Melee":
                    ShowSpellSprite(_results[0], _spellResults[1]);
                    break;
                case "Projectile":
                    ShowSpellSprite(_results[0], _spellResults[0]);
                    break;
                case "Explosion":
                    ShowSpellSprite(_results[0], _spellResults[2]);
                    break;
                case "Buff":
                    ShowSpellSprite(_results[0], _spellResults[3]);
                    break;
            }
        }

        public void ShowElementsResult(AElementalRune[] runes)
        {
            switch(runes.Length)
            {
                case 1:
                    switch (runes[0].Name)
                    {
                        case "Fire":
                            ShowSpellSprite(_results[0], _spellResults[4]);
                            break;
                        case "Electric":
                            ShowSpellSprite(_results[0], _spellResults[5]);
                            break;
                        case "Cosmic":
                            ShowSpellSprite(_results[0], _spellResults[6]);
                            break;
                        case "Phantom":
                            ShowSpellSprite(_results[0], _spellResults[7]);
                            break;
                    }
                    break;
                case 2:
                    for (int i = 0; i < runes.Length; i++)
                    {
                        switch (runes[i].Name)
                        {
                            case "Fire":
                                ShowSpellSprite(_results[i + 1], _spellResults[4]);
                                break;
                            case "Electric":
                                ShowSpellSprite(_results[i + 1], _spellResults[5]);
                                break;
                            case "Cosmic":
                                ShowSpellSprite(_results[i + 1], _spellResults[6]);
                                break;
                            case "Phantom":
                                ShowSpellSprite(_results[i + 1], _spellResults[7]);
                                break;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < runes.Length; i++)
                    {
                        switch (runes[i].Name)
                        {
                            case "Fire":
                                ShowSpellSprite(_results[i + 3], _spellResults[4]);
                                break;
                            case "Electric":
                                ShowSpellSprite(_results[i + 3], _spellResults[5]);
                                break;
                            case "Cosmic":
                                ShowSpellSprite(_results[i + 3], _spellResults[6]);
                                break;
                            case "Phantom":
                                ShowSpellSprite(_results[i + 3], _spellResults[7]);
                                break;
                        }
                    }
                    break;
            }
        }

        void ShowSpellSprite(Image img, Sprite spr)
        {
            img.sprite = spr;
            img.GetComponent<RectTransform>().DOScale(0.55f, 0.2f).Play().OnComplete(() => img.GetComponent<RectTransform>().DOScale(0.5f, 0.7f).Play().OnComplete(() => 
                img.GetComponent<RectTransform>().localScale = new Vector3(0.45f, 0.45f, 0.45f)));
            img.DOFade(1f, 0.2f).Play().OnComplete(() => img.DOFade(0f, 0.7f).Play());
        }
    }
}
