using Dialogues;
using Events;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace SpellInteractuable
{
    public class SI_AltWoodDestruction : ASpellInteractuable
    {
        [SerializeField] DialogueAgent _npc;
        [SerializeField] Dialogue _newDialogue;
        ExploreEventManager _event;

        private void Awake()
        {
            _event = FindObjectOfType<ExploreEventManager>();
        }

        protected override void ApplyInteraction(ASpell spell)
        {
            _event.LoadEventResult(EEventSolution.AltCompleted);
            _npc.ChangeDialogue(_newDialogue);
            _npc.StartDialogue();
            gameObject.SetActive(false);
            _npc.gameObject.SetActive(false);
        }
    }
}