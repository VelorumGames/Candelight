using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    private void Awake()
    {
        if (Random.value < 0.35f) gameObject.SetActive(false);
    }
}
