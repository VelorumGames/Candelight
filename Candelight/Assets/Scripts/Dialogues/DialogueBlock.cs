using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "NPC Dialogue/DialogueBlock")]
    public class DialogueBlock : ScriptableObject
    {
        public Sprite icon;
        [TextArea(15,30)] public string text;
        public GameObject item;
        public bool RandomItem;
        public DialogueBlock nextBlock;
        public Dialogue nextDialogue;
    }
}