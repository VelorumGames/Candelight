using Controls;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowInstructions : MonoBehaviour
    {
        [SerializeField] Sprite[] _instrSprites;
        [SerializeField] Sprite[] _elementSprites;
        [SerializeField] Image[] _imgs;
        [SerializeField] Image[] _elements;
        int _current;

        bool _activeCoroutine;

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
                for (int i = 0; i < Mage.Instance.GetActiveElements().Count; i++)
                {
                    _elements[i].gameObject.SetActive(true);
                    ARune rune = Mage.Instance.GetActiveElements()[i];
                    switch (rune.Name)
                    {
                        case "Fire":
                            _elements[i].sprite = _elementSprites[0];
                            break;
                        case "Electric":
                            _elements[i].sprite = _elementSprites[1];
                            break;
                        case "Cosmic":
                            _elements[i].sprite = _elementSprites[2];
                            break;
                        case "Phantom":
                            _elements[i].sprite = _elementSprites[3];
                            break;
                        default:
                            Debug.Log("ERROR: No se ha encontrado ningun sprite con este nombre: " + rune.Name);
                            break;
                    }
                }
            }
        }

        public void ResetElements()
        {
            foreach (var i in _elements)
            {
                i.sprite = null;
                i.gameObject.SetActive(false);
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
            Debug.Log("Reseteo sprites");
            _current = 0;

            foreach (var i in _imgs)
            {
                if (i != null)
                {
                    i.gameObject.SetActive(false);
                    i.color = Color.white;
                    i.sprite = null;
                }
            }
        }

        public IEnumerator ShowValidInstructions()
        {
            _activeCoroutine = true;
            foreach (var i in _imgs)
            {
                i.color = Color.red;
            }
            yield return new WaitForSeconds(1f);
            _activeCoroutine = false;
            ResetSprites();
        }
    }
}
