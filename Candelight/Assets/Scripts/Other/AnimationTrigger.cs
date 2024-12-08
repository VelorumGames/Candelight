using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    bool _triggered;

    Animator _anim;
    AudioSource _audio;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_triggered)
        {
            _triggered = true;
            _anim.SetTrigger("Trigger");
            _audio?.Play();
        }
    }
}
