using Dialogues;
using Map;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class PrisonerMonsterFailCinematic : MonoBehaviour
    {
        public DialogueAgent Agent;
        public Dialogue[] Dialogues;
        int count;
        int cinematic = 0;

        [SerializeField] NPCController[] _npcs;

        PlayerController _player;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            Agent.GetComponent<PlayDialogueInRoom>().LoadEndAction(() => StartCoroutine(ManageCinematic()));
        }

        public void NextDialogue()
        {
            if (count < Dialogues.Length)
            {
                Agent.LoadActionOnEnd(() => StartCoroutine(ManageCinematic()));

                Agent.ChangeDialogue(Dialogues[count++]);
                Agent.StartDialogue();
            }
            else
            {
                foreach(var npc in _npcs)
                {
                    StartCoroutine(npc.ExitRoom());
                }
            }
        }

        IEnumerator ManageCinematic()
        {
            yield return new WaitForSeconds(1f);

            switch(cinematic++)
            {
                case 0:
                    yield return StartCoroutine(Cinematic0());
                    break;
                case 1:
                    yield return StartCoroutine(Cinematic1());
                    break;
            }

            NextDialogue();
        }

        IEnumerator Cinematic0()
        {
            yield return Agent.GetComponent<NPCController>().MoveTowards(_player.transform.position, 0.5f);
        }

        IEnumerator Cinematic1()
        {
            foreach (var npc in _npcs) StartCoroutine(npc.MoveTowards(_player.transform.position, 1f));
            yield return new WaitForSeconds(1f);
        }
    }
}