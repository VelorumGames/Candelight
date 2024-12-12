using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BruteCalmDisappear : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "CalmScene") gameObject.SetActive(false);
    }
}
