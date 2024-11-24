using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ElementFloat : MonoBehaviour
    {
        RectTransform _trans;
        [SerializeField] float _range;
        [SerializeField] float _speed;

        private void Awake()
        {
            _trans = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _trans.localPosition = new Vector3(_trans.localPosition.x, Mathf.PingPong(Time.time * _speed, _range) - _range * 0.5f, _trans.localPosition.z);
        }
    }
}
