using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogues;

namespace Events
{
    public class HonorHeridoEnd : MonoBehaviour
    {
        [SerializeField] Dialogue _ignoredDialogue;
        [SerializeField] Dialogue _failedDialogue;

        DialogueAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<DialogueAgent>();
        }

        private void Start()
        {
            _agent.ChangeDialogue(FindObjectOfType<CalmEventManager>().GetEventSolution() == World.EEventSolution.Failed ? _failedDialogue : _ignoredDialogue);
        }
    }
}
