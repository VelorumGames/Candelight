using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBigBlock : MonoBehaviour
{
    int count;

    public void RegisterCount()
    {
        if (++count >= 2)
        {
            gameObject.SetActive(false);
        }
    }
}
