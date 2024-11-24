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
            if (Dialogue == null) Dialogue = Reward ? FindObjectOfType<RandomDialogues>().GetRewardDialogue() : RandomDialogue ? FindObjectOfType<RandomDialogues>().GetRandomDialogue() : null;
        }

        public void StartDialogue()
        {
            if (Dialogue != null)
            {
                if (_endAction == null && _startAction == null) _dialogueUI.StartDialogue(Dialogue, this);
                else if (_startAction != null)
                {
                    _dialogueUI.StartDialogueWithAction(Dialogue, this, _startAction);
                    _startAction = null;
                }
                else if (_endAction != null)
                {
                    Debug.Log("Se ejecuta dialogo con accion final");
                    _dialogueUI.StartDialogue(Dialogue, this, _endAction);
                    _endAction = null;
                }

                if (!Dialogue.Loop) Dialogue = null;

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

        public void LoadActionOnEnd(Action act)
        {
            Debug.Log("CARGAMOS ACCION");
            _endAction = act;
        }
        public void LoadActionOnStart(Action act) => _startAction = act;
    }
}