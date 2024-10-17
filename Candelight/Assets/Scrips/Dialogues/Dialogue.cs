using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "NPC Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public DialogueBlock initialDialogueBlock;
    }
}