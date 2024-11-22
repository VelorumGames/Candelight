using Dialogues;
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
            Agent.LoadActionOnEnd(() => StartCoroutine(ManageCinematic()));
        }

        public void NextDialogue()
        {
            if (count <= Dialogues.Length - 1) Agent.LoadActionOnEnd(() => StartCoroutine(ManageCinematic()));

            Agent.ChangeDialogue(Dialogues[count++]);
            Agent.StartDialogue();
        }

        IEnumerator ManageCinematic()
        {
            yield return new WaitForSeconds(1f);
            NextDialogue();
        }
    }
}