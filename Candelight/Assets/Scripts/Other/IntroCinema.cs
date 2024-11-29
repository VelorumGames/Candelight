using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroCinema : MonoBehaviour
{
    [SerializeField] Sprite[] _cinems;
    Image _img;
    TextMeshProUGUI _text;

    private void Awake()
    {
        _img = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public IEnumerator ShowCinematic(int id, string s, float duration)
    {
        if (_cinems[id] != null)
        {
            _img.color = Color.white;
            _img.sprite = _cinems[id];
            _text.text = s;
            _cinems[id] = null;

            yield return new WaitForSeconds(duration);

            _img.color = new Color(1f, 1f, 1f, 0f);
            _text.text = "";
        }
    }
}
