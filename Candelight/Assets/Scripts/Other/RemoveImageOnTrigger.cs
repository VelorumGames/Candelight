using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveImageOnTrigger : MonoBehaviour
{
    public Image Img;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Img.DOFade(0f, 4f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
