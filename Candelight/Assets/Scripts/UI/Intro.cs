using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public bool Alt;

    Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    private IEnumerator Start()
    {
        if (Alt)
        {
            _img.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.5f);
            _img.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        _img.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
