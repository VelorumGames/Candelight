using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    [SerializeField] float _speed;
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, _speed * 0.1f);
    }
}
