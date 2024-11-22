using Dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class MrBombasticLeave : MonoBehaviour
    {
        public DialogueAgent Subordinate;
        public Dialogue NewDialogue;

        private void Start()
        {
            GetComponent<DialogueAgent>().LoadActionOnEnd(BossLeave);
        }

        void BossLeave()
        {
            Debug.Log("SE MARCHA LA JEFA");
            gameObject.SetActive(false);
            Subordinate.ChangeDialogue(NewDialogue);
            Subordinate.GetComponent<Collider>().enabled = true;
        }
    }
}