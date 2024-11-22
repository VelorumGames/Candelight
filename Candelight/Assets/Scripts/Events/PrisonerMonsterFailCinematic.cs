using Dialogues;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class PrisonerMonsterFailCinematic : MonoBehaviour
    {
        public DialogueAgent Agent;
        public Dialogue[] Dialogues;
        int count;

        private void Start()
        {
            Agent.GetComponent<PlayDialogueInRoom>().LoadEndAction(() => StartCoroutine(ManageCinematic()));
        }

        public void NextDialogue()
        {
            if (count < Dialogues.Length)
            {
                Agent.LoadActionOnEnd(() => StartCoroutine(ManageCinematic()));

                Agent.ChangeDialogue(Dialogues[count++]);
                Agent.StartDialogue();
            }
        }

        IEnumerator ManageCinematic()
        {
            yield return new WaitForSeconds(1f);
            NextDialogue();
        }
    }
}