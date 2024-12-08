using Dialogues;
using Events;
using Hechizos;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace SpellInteractuable
{
    public class SI_MuertoEnVida : ASpellInteractuable
    {
        ExploreEventManager _event;
        DialogueAgent _agent;

        public Dialogue DeathDialogue;
        public Dialogue LifeDialogue;

        private void Awake()
        {
            _event = FindObjectOfType<ExploreEventManager>();
            _agent = GetComponent<DialogueAgent>();
        }

        protected override void ApplyInteraction(ASpell spell)
        {
            if (spell.Elements.Length == 1 && spell.Elements[0].Name == "Cosmic") //Para asegurar que es solo el cosmico
            {
                _event.LoadEventResult(EEventSolution.AltCompleted);

                _agent.ChangeDialogue(LifeDialogue);
                _agent.StartDialogue();
            }
            else
            {
                _event.LoadEventResult(EEventSolution.Completed);

                _agent.ChangeDialogue(DeathDialogue);
                _agent.LoadActionOnEnd(Death);
                _agent.StartDialogue();
            }
        }

        void Death()
        {
            _agent.gameObject.SetActive(false);
            _agent.ChangeDialogue(null);
        }
    }
}