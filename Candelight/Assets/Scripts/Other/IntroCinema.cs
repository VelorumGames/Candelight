using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroCinema : MonoBehaviour
{
    [SerializeField] Sprite[] _cinems;
    Image _img;
    TextMeshProUGUI _text;

    Color _oColor;

    List<int> _takenIds = new List<int>();

    AudioSource _audio;

    private void Awake()
    {
        _img = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _audio = GetComponent<AudioSource>();

        _oColor = _img.color;
        
        _img.color = new Color(_oColor.r, _oColor.g, _oColor.b, 0f);
        _text.text = "";
        _text.DOFade(0f, 0.1f);
    }

    public IEnumerator ShowCinematic(float initialWait, int id, string s, float duration)
    {
        if (!_takenIds.Contains(id))
        {
            yield return new WaitForSeconds(initialWait);

            _img.sprite = _cinems[id];
            _img.DOFade(1f, 0.01f);

            _audio.Play();
            _audio.volume -= 0.05f;

            _text.text = s;
            _text.DOFade(1f, 0.01f);

            _takenIds.Add(id);

            yield return new WaitForSeconds(duration);

            _img.DOFade(0f, 2f);
            _text.DOFade(0f, 2f);
        }
    }
}
