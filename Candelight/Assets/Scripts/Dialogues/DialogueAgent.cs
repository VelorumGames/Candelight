using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class DialogueAgent : MonoBehaviour
    {
        public bool Reward;
        public bool RandomDialogue;

        public Dialogue Dialogue;
        
        DialogueUI _dialogueUI;

        Action _endAction;
        Action _startAction;

        private void Awake()
        {
            _dialogueUI = FindObjectOfType<DialogueUI>();
        }

        private void Start()
        {
            if (Dialogue == null) Dialogue = Reward ? FindObjectOfType<RandomDialogues>().GetRewardDialogue() : RandomDialogue ? FindObjectOfType<RandomDialogues>().GetLoreDialogue() : null;
        }

        public void StartDialogue()
        {
            if (Dialogue != null)
            {
                if (_endAction == null && _startAction == null) _dialogueUI.StartDialogue(Dialogue.initialDialogueBlock, this);
                else if (_startAction != null)
                {
                    _dialogueUI.StartDialogueWithAction(Dialogue.initialDialogueBlock, this, _startAction);
                    _startAction = null;
                }
                else
                {
                    _dialogueUI.StartDialogue(Dialogue.initialDialogueBlock, this, _endAction);
                    _endAction = null;
                }

                //TODO
                //Vamos a probar a ver si esto funciona bien
                GetComponent<Collider>().enabled = false;
            }
            else Debug.LogWarning($"{gameObject.name} has not found the dialogue data. Execution will continue but will not work properly.");
        }

        public void ChangeDialogue(Dialogue newDialogue)
        {
            GetComponent<Collider>().enabled = true;
            Dialogue = newDialogue;
        }

        public void LoadActionOnEnd(Action act) => _endAction = act;
        public void LoadActionOnStart(Action act) => _startAction = act;
    }
}