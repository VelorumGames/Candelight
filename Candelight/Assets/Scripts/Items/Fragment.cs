using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Fragment : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<Inventory>().AddFragments(1);
                Destroy(gameObject);
            }
        }
    }
}