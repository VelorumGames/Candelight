using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowInstructions : MonoBehaviour
    {
        [SerializeField] Sprite[] _instrSprites;
        [SerializeField] Image[] _imgs;
        int _current;

        private void Start()
        {
            ResetSprites();
        }

        public void ShowInstruction(ESpellInstruction instr)
        {
            StopAllCoroutines();

            if (_current == 0) ResetSprites();

            _imgs[_current].gameObject.SetActive(true);
            _imgs[_current++].sprite = _instrSprites[(int)instr];

            _current %= _imgs.Length;
        }

        public void ResetSprites()
        {
            _current = 0;

            foreach (var i in _imgs)
            {
                i.gameObject.SetActive(false);
                i.color = Color.white;
                i.sprite = null;
            }
        }

        public IEnumerator ShowValidInstructions()
        {
            foreach (var i in _imgs)
            {
                i.color = Color.red;
            }
            yield return new WaitForSeconds(1f);
            ResetSprites();
        }
    }
}
