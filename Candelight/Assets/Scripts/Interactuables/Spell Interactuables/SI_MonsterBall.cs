using Dialogues;
using Events;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellInteractuable
{
    public class SI_MonsterBall : ASpellInteractuable
    {
        public DialogueAgent Agent;
        public Dialogue[] DestroyDialogues;
        public Dialogue LiberatedDialogue;
        int count;

        public PrisonerMonsterFail Prisoner;

        protected override void ApplyInteraction(ASpell spell)
        {
            if (count < 2)
            {
                Agent.ChangeDialogue(DestroyDialogues[count++]);
                Agent.StartDialogue();
            }
            else
            {
                Agent.ChangeDialogue(LiberatedDialogue);
                Prisoner.UnloadCall();
                FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Completed);
                gameObject.SetActive(false);
            }
        }
    }
}