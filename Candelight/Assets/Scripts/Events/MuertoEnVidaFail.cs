using Dialogues;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Events
{
    public class MuertoEnVidaFail : MonoBehaviour
    {
        DialogueAgent _agent;
        ARoom _room;

        public Dialogue ExitDialogue;

        private void Awake()
        {
            _agent = GetComponent<DialogueAgent>();
            _room = transform.parent.parent.parent.parent.GetComponent<ARoom>();
        }

        private void Start()
        {
            _agent.LoadActionOnEnd(SetFailState);
        }

        void SetFailState()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.Failed);
            _room.OnPlayerExit += PlayFailExitDialogue;
        }

        void PlayFailExitDialogue()
        {
            _agent.ChangeDialogue(ExitDialogue);
            _agent.StartDialogue();

            _room.OnPlayerExit -= PlayFailExitDialogue;
        }

        private void OnDisable()
        {
            _room.OnPlayerExit -= PlayFailExitDialogue;
        }
    }
}