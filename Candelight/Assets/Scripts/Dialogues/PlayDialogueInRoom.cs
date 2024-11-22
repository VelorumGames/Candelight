using Dialogues;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Map
{
    public class PlayDialogueInRoom : MonoBehaviour
    {
        [SerializeField] Dialogue _dialogue;
        ARoom _room;

        private void Awake()
        {
            _room = GetComponentInParent<ARoom>();
        }

        private void OnEnable()
        {
            _room.OnPlayerEnter += PlayDialogue;
        }

        void PlayDialogue()
        {
            FindObjectOfType<DialogueUI>().StartDialogue(_dialogue, GetComponent<DialogueAgent>());
            _room.OnPlayerEnter -= PlayDialogue;
        }

        private void OnDisable()
        {
            _room.OnPlayerEnter -= PlayDialogue;
        }
    }
}
