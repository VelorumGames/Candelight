using Dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public class NpcInter : AInteractuables
    {

        DialogueAgent _dialogue;

        private void Awake()
        {
            _dialogue = GetComponent<DialogueAgent>();
        }

        public override void Interaction()
        {
            _dialogue.StartDialogue();
        }
    }
}
