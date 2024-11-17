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
                _cf = other.gameObject.AddComponent<ConstantForce>();
                _cf.force = new Vector3(0f, -500f, 0f);

                _cf.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                _active = false;
            }
        }
    }

    private void OnDisable()
    {
        if (_cf != null)
        {
            _cf.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            Destroy(_cf);
        }
    }
}
