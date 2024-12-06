using Dialogues;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class CalmMuertoEnVidaEvent : MonoBehaviour
    {
        DialogueAgent _agent;
        public Dialogue SeeDialogue;

        [SerializeField] NPCController[] _npcs;

        private void Awake()
        {
            _agent = GetComponent<DialogueAgent>();
        }

        private void Start()
        {
            _agent.LoadActionOnEnd(() => StartCoroutine(Cinematic()));
        }

        IEnumerator Cinematic()
        {
            //Personajes miran al jugador
            foreach (var npc in _npcs)
            {
                StartCoroutine(npc.MoveTowards(FindObjectOfType<PlayerController>().transform.position, 0.5f));
            }

            yield return new WaitForSeconds(1.5f);

            _agent.ChangeDialogue(SeeDialogue);

            _agent.LoadActionOnEnd(End);
            _agent.StartDialogue();
        }

        void End()
        {
            //Personajes se piran
            foreach (var npc in _npcs)
            {
                StartCoroutine(npc.ExitRoom());
            }
        }
    }
}
