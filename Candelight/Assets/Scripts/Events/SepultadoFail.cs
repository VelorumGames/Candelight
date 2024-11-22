using Dialogues;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class SepultadoFail : MonoBehaviour
    {
        ARoom _room;
        public Dialogue ExitDialogue;

        private void Awake()
        {
            _room = transform.parent.parent.parent.parent.GetComponent<ARoom>();
        }

        private void Start()
        {
            _room.OnPlayerExit += SetFailState;
        }

        void SetFailState()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Failed);
            GetComponent<DialogueAgent>().ChangeDialogue(ExitDialogue);
            GetComponent<DialogueAgent>().StartDialogue();

            _room.OnPlayerExit -= SetFailState;
        }

        private void OnDisable()
        {
            _room.OnPlayerExit -= SetFailState;
        }
    }
}