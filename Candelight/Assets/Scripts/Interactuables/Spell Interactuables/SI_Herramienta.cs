using Dialogues;
using Events;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace SpellInteractuable
{
    public class SI_Herramienta : ASpellInteractuable
    {
        int _count = 0;

        public Dialogue[] AttackDialogues;
        DialogueAgent _agent;
        public GameObject HombreDeCobreEnemy;

        private void Awake()
        {
            _agent = GetComponent<DialogueAgent>();
        }

        protected override void ApplyInteraction(ASpell spell)
        {
            switch(++_count)
            {
                case 1:
                    _agent.ChangeDialogue(AttackDialogues[0]);
                    _agent.StartDialogue();
                    break;
                case 2:
                    _agent.ChangeDialogue(AttackDialogues[1]);
                    _agent.StartDialogue();
                    break;
                case 3:
                    _agent.ChangeDialogue(AttackDialogues[2]);
                    _agent.LoadActionOnEnd(StartFight);
                    _agent.StartDialogue();
                    break;
            }
        }

        void StartFight()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.AltCompleted);

            Instantiate(HombreDeCobreEnemy, _agent.transform.position, Quaternion.identity);
            Destroy(_agent.transform.parent.gameObject);
        }
    }
}