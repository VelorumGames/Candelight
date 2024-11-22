using Dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class MrBombasticSubLeave : MonoBehaviour
    {
        public GameObject Subordinate;
        public Dialogue NewDialogue;

        private void Start()
        {
            GetComponent<DialogueAgent>().LoadActionOnEnd(SubLeave);
        }

        void SubLeave()
        {
            Debug.Log("SE MARCHA EL SUBORDINADO");
            Subordinate.SetActive(false);
            GetComponent<DialogueAgent>().ChangeDialogue(NewDialogue);
        }
    }
}