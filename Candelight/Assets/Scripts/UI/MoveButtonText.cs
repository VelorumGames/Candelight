using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButtonText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    TextMeshProUGUI _text;
    [SerializeField] float _offset;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _text.rectTransform.localPosition = new Vector3(_text.rectTransform.localPosition.x, _text.rectTransform.localPosition.y - _offset, _text.rectTransform.localPosition.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _text.rectTransform.localPosition = new Vector3(_text.rectTransform.localPosition.x, _text.rectTransform.localPosition.y + _offset, _text.rectTransform.localPosition.z);
    }
}
