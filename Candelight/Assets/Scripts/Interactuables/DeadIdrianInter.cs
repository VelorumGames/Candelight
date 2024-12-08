using Dialogues;
using Events;
using Items;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Interactuables
{
    public class DeadIdrianInter : AInteractuables
    {
        Inventory _inv;
        public GameObject VialIdriano;

        public Dialogue DeadEnterDialogue;
        public Dialogue RevealDialogue;

        DialogueAgent _agent;

        public GameObject HombreDeCobreEnemy;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
        }

        public override void Interaction()
        {
            string itemName = _inv.AddItem(VialIdriano, EItemCategory.Epic);

            SpawnEventObject deadEvent = FindObjectOfType<SpawnEventObject>();
            deadEvent.GetComponentInChildren<PlayDialogueInRoom>().ChangeDialogue(DeadEnterDialogue);

            _agent = deadEvent.GetComponent<DialogueAgent>();
            _agent.ChangeDialogue(RevealDialogue);
            _agent.LoadActionOnStart(() => _inv.RemoveItem(itemName));
            _agent.LoadActionOnEnd(StartFight);

            FindObjectOfType<ExploreEventManager>().LoadEventResult(EEventSolution.Completed);
        }

        void StartFight()
        {
            Instantiate(HombreDeCobreEnemy, _agent.transform.position, Quaternion.identity);
            Destroy(_agent.transform.parent.gameObject);
        }
    }
}