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

        [SerializeField] GameObject _dialogueCanvas;

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
            _inventory = Inventory.Instance;
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
                    case EBiome.A:
                        _currentBlock.item = Inventory.Instance.GetRandomItem(EItemCategory.Common);
                        break;
                    case EBiome.B:
                        _currentBlock.item = Inventory.Instance.GetRandomItem(EItemCategory.Rare);
                        break;
                    case EBiome.C:
                        _currentBlock.item = Inventory.Instance.GetRandomItem(EItemCategory.Epic);
                        break;
                    default:
                        Debug.Log("ERROR: Bioma no detectado correctamente. Se defaultea item a common.");
                        _currentBlock.item = Inventory.Instance.GetRandomItem(EItemCategory.Common);
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

        public bool IsActive() => _active;
    }
}