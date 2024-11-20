using DG.Tweening;
using Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFragments : MonoBehaviour
    {
        Inventory _inv;
        [SerializeField] Image _sprite;
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] TextMeshProUGUI _sumText;

        Vector3 _prevPos;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();

            _prevPos = _sumText.GetComponent<RectTransform>().localPosition;
        }

        private void OnEnable()
        {
            if (_inv != null) _inv.OnFragmentsChange += UpdateFragments;
        }

        void UpdateFragments(int prev, int num)
        {
            _sumText.DOFade(1f, 0.2f).Play();
            _text.DOFade(1f, 0.2f).Play();
            _sprite.DOFade(1f, 0.2f).Play();

            _sumText.text = num - prev > 0 ? $"+{num - prev}" : $"-{num - prev}";
            _sumText.DOFade(0f, 1f).Play();
            _sumText.GetComponent<RectTransform>().DOLocalMoveY(38f, 1f).Play().OnComplete(() => _sumText.GetComponent<RectTransform>().localPosition = _prevPos);

            StopAllCoroutines();
            StartCoroutine(ManageFragmentCount(prev, num));
        }

        IEnumerator ManageFragmentCount(int prev, int num)
        {
            Debug.Log($"Actual: {prev}; Objetivo: {num}");
            while (prev != num)
            {
                _text.text = $"{prev}";

                if (num > prev) prev++;
                else prev--;

                yield return new WaitForSeconds(0.2f);
            }

            _text.text = $"{prev}";

            yield return new WaitForSeconds(3f);

            _sprite.DOFade(0f, 0.5f).Play();
            _text.DOFade(0f, 0.5f).Play();
        }

        private void OnDisable()
        {
            if (_inv != null) _inv.OnFragmentsChange -= UpdateFragments;
        }
    }
}