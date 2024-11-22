using Dialogues;
using Items;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class MrBombasticFail : MonoBehaviour
    {
        Inventory _inv;
        DialogueAgent _agent;
        public Dialogue FailDialogue;
        public Dialogue CompletedDialogue;
        public Dialogue FarDialogue;

        ARoom _room;

        bool _completed;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _agent = GetComponent<DialogueAgent>();

            _room = transform.parent.parent.parent.parent.GetComponent<ARoom>();
        }

        private void Start()
        {
            _agent.LoadActionOnEnd(CheckEventState);
        }

        void CheckEventState()
        {
            if (_inv.FindItem("Bomba de Pólvora", out AItem item) && (int)item.Data.Category <= _inv.GetFragments())
            {
                _agent.ChangeDialogue(FailDialogue);
                FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Failed);
                Invoke("StartFailDialogue", 2f);
            }
            else
            {
                //NPC -> Evento -> SP -> SP Container -> Sala
                _room.OnPlayerExit += LoadFarDialogue;
                StartCoroutine(CheckForItemActivation(item));
            }
        }

        IEnumerator CheckForItemActivation(AItem item)
        {
            yield return new WaitUntil(() => item.IsActive());
            _agent.ChangeDialogue(CompletedDialogue);
            _agent.LoadActionOnStart(SetCompleted);
        }

        void SetCompleted()
        {
            FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.Completed);
            _completed = true;
            _inv.RemoveItem("Bomba de Pólvora");
        }

        public void StartFailDialogue() => _agent.StartDialogue();

        void LoadFarDialogue()
        {
            StopAllCoroutines();

            _agent.ChangeDialogue(FarDialogue);
            _agent.StartDialogue();
            _room.OnPlayerExit -= LoadFarDialogue;

            //Si lo activa y entonces sale del rango, tenemos que volver a estar pendientes de si le da el item o no
            if (_inv.FindItem("Bomba de Pólvora", out AItem item)) StartCoroutine(CheckForItemActivation(item));
        }

        private void OnDisable()
        {
            if (!_completed) FindObjectOfType<ExploreEventManager>().LoadEventResult(World.EEventSolution.AltCompleted);
            _room.OnPlayerExit -= LoadFarDialogue;
        }
    }
}