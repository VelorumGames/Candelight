using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Auxiliar;
using UnityEngine.UI;
using Controls;
using Player;

namespace Dialogues
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] GameObject _textGameObject;
        [SerializeField] GameObject _iconGameObject;

        bool _active = false;

        TextMeshProUGUI _text;
        ShowUITextScript _showUIText;
        DialogueBlock _currentBlock;

        [SerializeField] GameObject _dialogueCanvas;

        PlayerController _cont;

        Image _spriteRend;

        private void Awake()
        {
            _text = _textGameObject.GetComponent<TextMeshProUGUI>();
            _showUIText = _textGameObject.GetComponent<ShowUITextScript>();

            _spriteRend = _iconGameObject.GetComponent<Image>();

            _cont = FindObjectOfType<PlayerController>();
        }

        private void LoadBlockInfo(DialogueBlock block)
        {
            _currentBlock = block;

            _showUIText.ShowText(_currentBlock.text);
            _spriteRend.sprite = _currentBlock.icon;
        }

        private void NextBlock()
        {
            if (_currentBlock.nextBlock != null) LoadBlockInfo(_currentBlock.nextBlock);
            else EndDialogue();
        }

        public void StartDialogue(DialogueBlock block)
        {
            _dialogueCanvas.SetActive(true);

            _active = true;
            _cont.UnloadInteraction();
            InputManager.Instance.LoadControls(EControlMap.Dialogue);

            LoadBlockInfo(block);
        }
        public void Next()
        {
            if (_showUIText.IsShowingText()) _showUIText.Interrupt();
            else NextBlock();
        }
        public void EndDialogue()
        {
            _active = false;
            InputManager.Instance.LoadPreviousControls();

            _text.text = "";

            _dialogueCanvas.SetActive(false);
        }

        //private void Update()
        //{
        //    //Debug
        //    if(_active)
        //    {
        //        if(Input.GetKeyDown(KeyCode.G))
        //        {
        //            if (_showUIText.IsShowingText()) _showUIText.Interrupt();
        //            else NextBlock();
        //        }
        //    }
        //}

        public bool IsActive() => _active;
    }
}