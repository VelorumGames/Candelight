using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelIntroMessage : MonoBehaviour
{
    TextMeshProUGUI _text;
    RectTransform _rect;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _rect = GetComponent<RectTransform>();
        _text.DOFade(0f, 0.1f);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);
        float oScale = _rect.localScale.x;
        _text.DOFade(1f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            _rect.DOScale(oScale * 1.3f, 2f).SetUpdate(true);
            _text.DOFade(0f, 2f).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
        });
    }
}
