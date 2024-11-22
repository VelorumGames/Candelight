using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpriteOrderManager : MonoBehaviour
{
    SpriteRenderer _rend;

    private void Awake()
    {
        _rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _rend.sortingOrder = -(int)transform.position.z;
        
    }
}
