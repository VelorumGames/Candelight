using Dialogues;
using Map;
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
            GetComponent<PlayDialogueInRoom>().LoadEndAction(SubLeave);
        }

        void SubLeave()
        {
            Debug.Log("SE MARCHA EL SUBORDINADO");

            StartCoroutine(Subordinate.GetComponent<NPCController>().ExitRoom());

            GetComponent<DialogueAgent>().ChangeDialogue(NewDialogue);
        }
    }
}