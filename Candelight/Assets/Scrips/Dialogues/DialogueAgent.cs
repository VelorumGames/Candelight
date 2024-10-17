using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class DialogueAgent : MonoBehaviour
    {
        public Dialogue dialogue;
        DialogueUI _dialogueUI;

        private void Awake()
        {
            _dialogueUI = FindObjectOfType<DialogueUI>();
        }

        private void Start()
        {
            //if (_dialogueUI != null) StartDialogue();
            //else Debug.LogWarning($"{this.name} has not found the DialogueUI script. Execution will continue but will not work properly.");
        }

        public void StartDialogue()
        {
            if (dialogue != null) _dialogueUI.StartDialogue(dialogue.initialDialogueBlock);
            else Debug.LogWarning($"{this.name} has not found the dialogue data. Execution will continue but will not work properly.");
        }
    }
}