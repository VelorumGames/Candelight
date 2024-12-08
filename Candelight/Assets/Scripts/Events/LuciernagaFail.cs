using Dialogues;
using Items;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Events
{
    public class LuciernagaFail : MonoBehaviour
    {
        bool _active;
        Inventory _inv;
        DialogueAgent _agent;

        public Dialogue CompletedDialogue;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _agent = GetComponent<DialogueAgent>();
        }

        private void Start()
        {
            _agent.LoadActionOnEnd(SetFailState);
        }

        void SetFailState()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.Failed);
            _active = true;
        }

        void SetCompletedState()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.Completed);
            _inv.RemoveItem("Luciernaga");
        }

        private void Update()
        {
            if (_active)
            {
                if (_inv.FindItem("Luciernaga", out AItem item))
                {
                    _agent.ChangeDialogue(CompletedDialogue);
                    _agent.LoadActionOnEnd(SetCompletedState);
                    _active = false;
                }
            }
        }
    }
}