using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Auxiliar;
using UnityEngine.UI;
using Controls;
using Player;
using Items;
using World;

namespace Dialogues
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] GameObject _textGameObject;
        [SerializeField] GameObject _iconGameObject;

        [SerializeField] NodeInfo _currentNodeInfo;

        bool _active = false;

        TextMeshProUGUI _text;
        ShowUITextScript _showUIText;
        DialogueBlock _currentBlock;
        DialogueAgent _currentAgent;

        [SerializeField] GameObject _dialogueUI;

        PlayerController _cont;

        Image _spriteRend;

        Inventory _inventory;

        private void Awake()
        {
            _text = _textGameObject.GetComponent<TextMeshProUGUI>();
            _showUIText = _textGameObject.GetComponent<ShowUITextScript>();

            _spriteRend = _iconGameObject.GetComponent<Image>();

            _cont = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            _inventory = FindObjectOfType<Inventory>();
        }

        private void LoadBlockInfo(DialogueBlock block)
        {
            _currentBlock = block;

            _showUIText.ShowText(_currentBlock.text);
            _spriteRend.sprite = _currentBlock.icon;

            if (_currentBlock.RandomItem)
            {
                switch(_currentNodeInfo.Biome)
                {
                    case EBiome.Durnia:
                        _currentBlock.item = _inventory.GetRandomItem(EItemCategory.Common);
                        break;
                    case EBiome.Temeria:
                        _currentBlock.item = _inventory.GetRandomItem(EItemCategory.Rare);
                        break;
                    case EBiome.Idria:
                        _currentBlock.item = _inventory.GetRandomItem(EItemCategory.Epic);
                        break;
                    default:
                        Debug.Log("ERROR: Bioma no detectado correctamente. Se defaultea item a common.");
                        _currentBlock.item = _inventory.GetRandomItem(EItemCategory.Common);
                        break;
                }
            }

            if (_currentBlock.item != null)
            {
                _inventory.AddItem(_currentBlock.item);
            }
        }

        private void NextBlock()
        {
            Debug.Log("Siguiente bloque");
            if (_currentBlock.nextBlock != null) LoadBlockInfo(_currentBlock.nextBlock);
            else
            {
                EndDialogue();
                if (_currentBlock.nextDialogue != null) _currentAgent.ChangeDialogue(_currentBlock.nextDialogue);
            }
        }

        public void StartDialogue(DialogueBlock block, DialogueAgent ag)
        {
            _currentAgent = ag;

            _dialogueUI.SetActive(true);

            _active = true;
            _cont.UnloadInteraction();
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Dialogue);

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
            FindObjectOfType<InputManager>().LoadPreviousControls();

            _text.text = "";

            _dialogueUI.SetActive(false);
        }

        public bool IsActive() => _active;
    }
}