using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class RandomDialogues : MonoBehaviour
    {
        public List<Dialogue> Dialogues;

        public Dialogue GetDialogue()
        {
            if (Dialogues.Count > 0)
            {
                Dialogue d = Dialogues[Random.Range(0, Dialogues.Count)];
                Dialogues.Remove(d);

                return d;
            }

            return null;
        }
    }
}
