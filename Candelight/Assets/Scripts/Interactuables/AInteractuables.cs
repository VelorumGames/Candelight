using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public abstract class AInteractuables : MonoBehaviour
    {
        public Transform NotificationTransform;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                
                other.GetComponentInParent<PlayerController>().LoadInteraction(Interaction, NotificationTransform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                
                other.GetComponentInParent<PlayerController>().UnloadInteraction();
            }
        }

        public abstract void Interaction();
    }
}
