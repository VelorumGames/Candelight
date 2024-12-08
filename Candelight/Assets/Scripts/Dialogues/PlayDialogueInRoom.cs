using Dialogues;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using System;

namespace Map
{
    public class PlayDialogueInRoom : MonoBehaviour
    {
        [SerializeField] Dialogue _dialogue;
        ARoom _room;

        Action _endAction;

        private void Awake()
        {
            _room = GetComponentInParent<ARoom>();
        }

        private void OnEnable()
        {
            _room.OnPlayerEnter += PlayDialogue;
        }

        void PlayDialogue()
        {
            if (_endAction != null)
            {
                FindObjectOfType<DialogueUI>().StartDialogue(_dialogue, GetComponent<DialogueAgent>(), _endAction);
            }
            else
            {
                FindObjectOfType<DialogueUI>().StartDialogue(_dialogue, GetComponent<DialogueAgent>());
            }
            _room.OnPlayerEnter -= PlayDialogue;
        }

        public void LoadEndAction(Action act)
        {
            _endAction = act;
        }

        public void ChangeDialogue(Dialogue dial)
        {
            _dialogue = dial;
            _room.OnPlayerEnter += PlayDialogue;
        }

        private void OnDisable()
        {
            _room.OnPlayerEnter -= PlayDialogue;
        }
    }
}
