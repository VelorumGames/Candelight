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
using System;
using Music;
using UI;

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
        Inventory _inventory;
        MusicManager _music;
        UISoundManager _sound;

        System.Action _onEndDialogue;

        Image _spriteRend;

        public event Action OnDialogueStart;
        public event Action OnDialogueEnd;

        private void Awake()
        {
            _text = _textGameObject.GetComponent<TextMeshProUGUI>();
            _showUIText = _textGameObject.GetComponent<ShowUITextScript>();

            _spriteRend = _iconGameObject.GetComponent<Image>();

            _cont = FindObjectOfType<PlayerController>();
            _music = FindObjectOfType<MusicManager>();
            _sound = FindObjectOfType<UISoundManager>();
        }

        private void Start()
        {
            _inventory = FindObjectOfType<Inventory>();
        }

        private void OnEnable()
        {
            OnDialogueStart += _music.StartDialogueMusic;
            OnDialogueEnd += _music.EndDialogueMusic;
        }

        private void LoadBlockInfo(DialogueBlock block)
        {
            _currentBlock = block;

            _showUIText.ShowText(_currentBlock.text);
            _spriteRend.sprite = _currentBlock.icon;
            _spriteRend.SetNativeSize();

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
                        _currentBlock.item = _inventory.GetRandomItem(UnityEngine.Random.value < 0.75f ? EItemCategory.Epic : EItemCategory.Legendary);
                        break;
                    default:
                        Debug.Log("ERROR: Bioma no detectado correctamente. Se defaultea item a common.");
                        _currentBlock.item = _inventory.GetRandomItem(EItemCategory.Common);
                        break;
                }
            }

            if (_currentBlock.item != null)
            {
                _inventory.AddItem(_currentBlock.item, EItemCategory.Rare);
            }
        }

        private void NextBlock()
        {
            Debug.Log("Siguiente bloque");
            if (_currentBlock.nextBlock != null)
            {
                LoadBlockInfo(_currentBlock.nextBlock);
                _sound.PlayDialogueNext();
            }
            else
            {
                EndDialogue();
                if (_currentBlock.nextDialogue != null && _currentAgent != null) _currentAgent.ChangeDialogue(_currentBlock.nextDialogue);
                else if (_onEndDialogue != null) _onEndDialogue();
                _sound.PlayDialogueEnd();
            }
        }

        public void StartDialogue(Dialogue dial, DialogueAgent ag)
        {
            Time.timeScale = 0f;
            _currentAgent = ag;

            _dialogueUI.SetActive(true);

            if(OnDialogueStart != null) OnDialogueStart();

            _active = true;
            _cont.UnloadInteraction();
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Dialogue);

            LoadBlockInfo(dial.initialDialogueBlock);
        }
        public void StartDialogue(Dialogue dial, DialogueAgent ag, System.Action endAction)
        {
            Time.timeScale = 0f;
            _currentAgent = ag;

            _dialogueUI.SetActive(true);

            if (OnDialogueStart != null) OnDialogueStart();

            _active = true;
            _cont.UnloadInteraction();
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Dialogue);

            _onEndDialogue = endAction;

            LoadBlockInfo(dial.initialDialogueBlock);
        }
        public void StartDialogueWithAction(Dialogue dial, DialogueAgent ag, System.Action action)
        {
            Time.timeScale = 0f;
            _currentAgent = ag;

            _dialogueUI.SetActive(true);

            if (OnDialogueStart != null) OnDialogueStart();

            _active = true;
            _cont.UnloadInteraction();
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Dialogue);

            if (action != null) action();

            LoadBlockInfo(dial.initialDialogueBlock);
        }
        public void Next()
        {
            if (_showUIText.IsShowingText()) _showUIText.Interrupt();
            else NextBlock();
        }
        public void EndDialogue()
        {
            Time.timeScale = 1f;

            _active = false;
            FindObjectOfType<InputManager>().LoadPreviousControls();
            if (OnDialogueEnd != null) OnDialogueEnd();
            if (_onEndDialogue != null) _onEndDialogue();
            _onEndDialogue = null;

            _text.text = "";

            _dialogueUI.SetActive(false);
        }

        public bool IsActive() => _active;

        private void OnDisable()
        {
            OnDialogueStart -= _music.StartDialogueMusic;
            OnDialogueEnd -= _music.EndDialogueMusic;
        }
    }
}