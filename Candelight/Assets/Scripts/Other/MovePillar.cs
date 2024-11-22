using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePillar : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] float _speed;
    Vector3 _oPos;

    private void Awake()
    {
        _oPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(_oPos.x, _oPos.y + Mathf.PingPong(Time.time * _speed, _range), _oPos.z);
    }
}
