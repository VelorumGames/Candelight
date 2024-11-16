using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantainAnchorRotation : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //Debug.Log(transform.rotation.eulerAngles);
    }
}
