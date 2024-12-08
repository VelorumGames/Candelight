using Dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Events
{
    public class HerramientaFail : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<DialogueAgent>().LoadActionOnEnd(() => FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.Failed));
        }
    }
}