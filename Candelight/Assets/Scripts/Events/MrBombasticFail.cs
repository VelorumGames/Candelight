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

        ExploreEventManager _event;

        ARoom _room;
        AItem _bomb;

        bool _completed;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _agent = GetComponent<DialogueAgent>();
            _event = FindObjectOfType<ExploreEventManager>();

            _room = transform.parent.parent.parent.parent.GetComponent<ARoom>();
            //NPC -> Evento -> SP -> SP Container -> Sala
        }

        private void Start()
        {
            _agent.LoadActionOnEnd(CheckEventState);
        }

        void CheckEventState()
        {
            //Debug.Log($"HE LLEGADO: {_inv.FindItem("Bomba de Pólvora", out _bomb)} && {(int)_bomb.Data.Category > _inv.GetFragments()} ({(int)_bomb.Data.Category} > {_inv.GetFragments()})");
            if (_inv.FindItem("Bomba de Pólvora", out _bomb) && (int)_bomb.Data.Category > _inv.GetFragments())
            {
                Invoke("StartFailDialogue", 1.5f);
            }
            else
            {
                _room.OnPlayerExit += LoadFarDialogue;
                StartCoroutine(CheckForItemActivation(_bomb));
            }
        }

        IEnumerator CheckForItemActivation(AItem item)
        {
            //Debug.Log("ESPERANDO: " + _bomb.gameObject.name);
            yield return new WaitUntil(() => item.IsActive());
            //Debug.Log("ACTIVADO");
            _agent.ChangeDialogue(CompletedDialogue);
            _agent.LoadActionOnStart(SetCompleted);
        }

        void SetCompleted()
        {
            _event.LoadEventResult(World.EEventSolution.Completed);
            _completed = true;
            _inv.RemoveItem("Bomba de Pólvora");
        }

        public void StartFailDialogue()
        {
            _agent.ChangeDialogue(FailDialogue);
            _agent.StartDialogue();
            
            _event.LoadEventResult(World.EEventSolution.Failed);
        }

        void LoadFarDialogue()
        {
            _agent.ChangeDialogue(FarDialogue);
            _agent.StartDialogue();
            _room.OnPlayerExit -= LoadFarDialogue;

            //Si lo activa y entonces sale del rango, tenemos que volver a estar pendientes de si le da el item o no
            //if (_inv.FindItem("Bomba de Pólvora", out AItem item)) StartCoroutine(CheckForItemActivation(item));
        }

        private void OnDisable()
        {
            if (!_completed) _event.LoadEventResult(World.EEventSolution.AltCompleted);
            _room.OnPlayerExit -= LoadFarDialogue;
        }
    }
}