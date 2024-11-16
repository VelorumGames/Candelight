using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class RandomDialogues : MonoBehaviour
    {
        public List<Dialogue> LoreDialogues;
        public List<Dialogue> ItemDialogues;

        public Dialogue GetLoreDialogue()
        {
            if (LoreDialogues.Count > 0)
            {
                Dialogue d = LoreDialogues[Random.Range(0, LoreDialogues.Count)];
                LoreDialogues.Remove(d);

                return d;
            }

            return null;
        }

        public Dialogue GetRewardDialogue()
        {
            if (ItemDialogues.Count > 0)
            {
                Dialogue d = ItemDialogues[Random.Range(0, ItemDialogues.Count)];
                ItemDialogues.Remove(d);

                return d;
            }

            return null;
        }
    }
}
