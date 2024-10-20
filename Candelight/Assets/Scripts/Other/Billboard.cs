using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;
    public bool RandomizePosition;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        if (RandomizePosition) transform.position += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
    }

    void LateUpdate()
    {
        if (mainCamera) transform.rotation = mainCamera.transform.rotation;
    }
}
