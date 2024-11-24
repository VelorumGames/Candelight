using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroGravity : MonoBehaviour
{
    ConstantForce _cf;
    bool _active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (_active)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<PlayerController>().SetMove(false);

                _cf = other.gameObject.AddComponent<ConstantForce>();
                _cf.force = new Vector3(0f, -500f, 0f);

                _cf.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                _active = false;

                Invoke("PlayParticles", 2f);
            }
        }
    }

    public void PlayParticles()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public void ResetPlayer()
    {
        if (_cf != null)
        {
            FindObjectOfType<PlayerController>().SetMove(true);
            _cf.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            _cf.force = new Vector3(0f, 0f, 0f);
            Destroy(_cf);
        }
    }
}
