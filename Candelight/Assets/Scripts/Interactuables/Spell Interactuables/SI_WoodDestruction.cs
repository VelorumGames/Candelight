using Dialogues;
using Events;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace SpellInteractuable
{
    public class SI_WoodDestruction : ASpellInteractuable
    {
        [SerializeField] DialogueAgent _npc;
        [SerializeField] Dialogue _newDialogue;

        private void Start()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.Ignored);
        }

        protected override void ApplyInteraction(ASpell spell)
        {
            ExploreEventManager.Instance.LoadEventResult(EEventSolution.Completed);
            _npc.ChangeDialogue(_newDialogue);
            Destroy(gameObject);
        }
    }
}