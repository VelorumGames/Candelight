using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visual
{
    public class RoomLights : MonoBehaviour
    {
        [SerializeField] GameObject[] _lights;

        private void Awake()
        {
            foreach (var l in _lights) if (l != null) l.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) foreach (var l in _lights) l.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) foreach (var l in _lights) l.SetActive(false);
        }
    }
}