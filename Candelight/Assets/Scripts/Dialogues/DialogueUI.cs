using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Auxiliar;
using UnityEngine.UI;

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

        Image _spriteRend;

        private void Awake()
        {
            _text = _textGameObject.GetComponent<TextMeshProUGUI>();
            _showUIText = _textGameObject.GetComponent<ShowUITextScript>();

            _spriteRend = _iconGameObject.GetComponent<Image>();
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
            _active = true;
            LoadBlockInfo(block);
        }
        public void EndDialogue()
        {
            _active = false;
            _text.text = "";
        }

        private void Update()
        {
            if(_active)
            {
                if(Input.GetKeyDown(KeyCode.G))
                {
                    if (_showUIText.IsShowingText()) _showUIText.Interrupt();
                    else NextBlock();
                }
            }
        }
    }
}