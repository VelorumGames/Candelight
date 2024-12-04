using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactuables
{
    public abstract class AInteractuables : MonoBehaviour
    {
        public Transform NotificationTransform;
        public GameObject Exclamation;

        public event Action OnTrigger;
        public event Action OnInteraction;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (OnTrigger != null) OnTrigger();
                other.GetComponentInParent<PlayerController>().LoadInteraction(EventInteraction, NotificationTransform);

                if (SceneManager.GetActiveScene().name == "TutorialScene")
                {
                    FindObjectOfType<UIManager>().ShowTutorial("Pulsa E para interactuar");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInParent<PlayerController>().UnloadInteraction();
            }
        }

        void EventInteraction()
        {
            if (Exclamation != null) Exclamation.SetActive(false);
            if (OnInteraction != null) OnInteraction();
            Interaction();
        }

        public abstract void Interaction();
    }
}
