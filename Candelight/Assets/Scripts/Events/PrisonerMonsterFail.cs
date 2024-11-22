using Dialogues;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class PrisonerMonsterFail : MonoBehaviour
    {
        ARoom _room;
        ARoom[] _adyRooms;
        DialogueAgent _agent;

        public Dialogue ExitDialogue;
        public Dialogue DistantDialogue;

        private void Awake()
        {
            _room = transform.parent.parent.parent.parent.GetComponent<ARoom>();
            _agent = GetComponent<DialogueAgent>();
        }

        private void OnEnable()
        {
            _agent.LoadActionOnEnd(PrepareFailState);
        }

        void PrepareFailState()
        {
            _room.OnPlayerExit += SetFailState;
        }

        void SetFailState()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Failed);
            _agent.ChangeDialogue(ExitDialogue);
            _agent.StartDialogue();

            _room.OnPlayerExit -= SetFailState;

            _adyRooms = _room.GetAdyacentRooms();
            foreach(var room in _adyRooms)
            {
                room.OnPlayerEnter += DistantCall;
            }
        }

        void DistantCall()
        {
            _agent.ChangeDialogue(DistantDialogue);
            _agent.StartDialogue();

            foreach (var room in _adyRooms)
            {
                room.OnPlayerEnter -= DistantCall;
            }
        }

        private void OnDisable()
        {
            _room.OnPlayerExit -= SetFailState;
            if (_adyRooms != null)
            {
                foreach (var room in _adyRooms)
                {
                    room.OnPlayerEnter -= DistantCall;
                }
            }
        }
    }
}