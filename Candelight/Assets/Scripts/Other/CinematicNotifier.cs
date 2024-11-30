using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicNotifier : MonoBehaviour
{
    IntroCinema _cine;

    public int Id;
    public float Duration;
    public string Text;

    private void Awake()
    {
        _cine = FindObjectOfType<IntroCinema>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Notify();
    }

    void Notify()
    {
        StartCoroutine(_cine.ShowCinematic(Id, Text, Duration));
    }
}
