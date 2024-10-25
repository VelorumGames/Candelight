using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class DialogueAgent : MonoBehaviour
    {
        public bool Reward;

        public Dialogue Dialogue;
        DialogueUI _dialogueUI;

        private void Awake()
        {
            _dialogueUI = FindObjectOfType<DialogueUI>();
        }

        private void Start()
        {
            if (!Dialogue) Dialogue = Reward ? FindObjectOfType<RandomDialogues>().GetRewardDialogue() : FindObjectOfType<RandomDialogues>().GetLoreDialogue();
        }

        public void StartDialogue()
        {
            if (Dialogue != null) _dialogueUI.StartDialogue(Dialogue.initialDialogueBlock);
            else Debug.LogWarning($"{this.name} has not found the dialogue data. Execution will continue but will not work properly.");
        }
    }
}