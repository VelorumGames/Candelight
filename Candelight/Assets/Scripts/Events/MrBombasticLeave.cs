using Dialogues;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class MrBombasticLeave : MonoBehaviour
    {
        public DialogueAgent Subordinate;
        public Dialogue NewDialogue;

        private void Start()
        {
            GetComponent<PlayDialogueInRoom>().LoadEndAction(BossLeave);
        }

        void BossLeave()
        {
            Debug.Log("SE MARCHA LA JEFA");

            StartCoroutine(GetComponent<NPCController>().ExitRoom());

            Subordinate.ChangeDialogue(NewDialogue);
            //Subordinate.GetComponent<Collider>().enabled = true;
        }
    }
}