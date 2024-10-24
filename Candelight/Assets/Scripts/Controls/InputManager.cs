using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;
using Dialogues;
using Hechizos;
using UI;
using Cameras;
using DG.Tweening;

namespace Controls
{
    public enum EControlMap
    {
        Level,
        World,
        Dialogue,
        UI
    }

    public enum ESpellInstruction
    {
        Up, Down, Right, Left
    }

    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public InputActionAsset Input;
        [SerializeField] EControlMap _initialControls;
        InputActionMap _prevControls;
        PlayerController _cont;

        InputActionMap _currentMap;
        InputActionMap _worldMap;
        InputActionMap _levelMap;
        InputActionMap _dialogueMap;
        InputActionMap _uiMap;

        DialogueUI _dialogue;

        InputAction _move;
        InputAction _choosePath;
        InputAction _element;
        InputAction _spell;
        InputAction _look;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            InitializeControls();

            DOTween.Init();
        }

        private void Start()
        {
            LoadControls(_initialControls);
        }

        void InitializeControls()
        {
            Debug.Log("Se inicializan los controles");
            _cont = FindObjectOfType<PlayerController>();

            if (_cont)
            {
                //Level
                _levelMap = Input.FindActionMap("Level");

                _move = _levelMap.FindAction("Move");
                InputAction interact = _levelMap.FindAction("Interact");
                interact.performed += _cont.OnInteract;
                _element = _levelMap.FindAction("Element");
                _element.performed += StartElementMode;
                _element.canceled += StopElementMode;
                _spell = _levelMap.FindAction("Spell");
                _spell.performed += StartSpellMode;
                _spell.canceled += StopSpellMode;
                InputAction spellUp = _levelMap.FindAction("SpellUp");
                spellUp.performed += RegisterSpellUp;
                InputAction spellDown = _levelMap.FindAction("SpellDown");
                spellDown.performed += RegisterSpellDown;
                InputAction spellRight = _levelMap.FindAction("SpellRight");
                spellRight.performed += RegisterSpellRight;
                InputAction spellLeft = _levelMap.FindAction("SpellLeft");
                spellLeft.performed += RegisterSpellLeft;
                InputAction book = _levelMap.FindAction("Book");
                book.performed += _cont.OnBook;
                InputAction levelPause = _levelMap.FindAction("Pause");
                levelPause.performed += _cont.OnPause;
                _look = _levelMap.FindAction("Look");

                //World
                _worldMap = Input.FindActionMap("World");

                _choosePath = _worldMap.FindAction("ChoosePath");
                InputAction confirmPath = _worldMap.FindAction("ConfirmPath");
                confirmPath.performed += _cont.OnConfirmPath;
                InputAction worldInteract = _worldMap.FindAction("Interact");
                worldInteract.performed += _cont.OnInteract;
                InputAction worldPause = _worldMap.FindAction("Pause");
                worldPause.performed += _cont.OnPause;

                //Dialogue
                _dialogueMap = Input.FindActionMap("Dialogue");
                _dialogue = FindObjectOfType<DialogueUI>();

                InputAction next = _dialogueMap.FindAction("Next");
                next.performed += NextDialogueBlock;
                InputAction dialoguePause = _dialogueMap.FindAction("Pause");
                dialoguePause.performed += _cont.OnPause;
            }

            //UI
            _uiMap = Input.FindActionMap("UI");

            InputAction back = _uiMap.FindAction("Back");
            back.performed += UIManager.Instance.OnUIBack;
            
        }

        public void LoadControls(EControlMap map)
        {
            //Debug.Log("Mapa previo: " + _currentMap);
            _prevControls = _currentMap;
            if (_currentMap != null) _currentMap.Disable();
            switch(map)
            {
                case EControlMap.Level:
                    _currentMap = _levelMap;
                    break;
                case EControlMap.World:
                    _currentMap = _worldMap;
                    break;
                case EControlMap.Dialogue:
                    _currentMap = _dialogueMap;
                    break;
                case EControlMap.UI:
                    _currentMap = _uiMap;
                    break;
            }
            //Debug.Log("Mapa colocado: " + _currentMap);
            _currentMap.Enable();
        }

        public void LoadPreviousControls()
        {
            _currentMap.Disable();
            _currentMap = _prevControls;
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            if (_cont)
            {
                InputAction interact = _levelMap.FindAction("Interact");
                interact.performed -= _cont.OnInteract;
                //_element = _levelMap.FindAction("Element");
                _element.performed -= StartElementMode;
                _element.canceled -= StopElementMode;
                //_spell = _levelMap.FindAction("Spell");
                _spell.performed -= StartSpellMode;
                _spell.canceled -= StopSpellMode;
                InputAction spellUp = _levelMap.FindAction("SpellUp");
                spellUp.performed -= RegisterSpellUp;
                InputAction spellDown = _levelMap.FindAction("SpellDown");
                spellDown.performed -= RegisterSpellDown;
                InputAction spellRight = _levelMap.FindAction("SpellRight");
                spellRight.performed -= RegisterSpellRight;
                InputAction spellLeft = _levelMap.FindAction("SpellLeft");
                spellLeft.performed -= RegisterSpellLeft;
                InputAction book = _levelMap.FindAction("Book");
                book.performed -= _cont.OnBook;
                InputAction levelPause = _levelMap.FindAction("Pause");
                levelPause.performed -= _cont.OnPause;
                _move.Disable();

                InputAction confirmPath = _worldMap.FindAction("ConfirmPath");
                confirmPath.performed -= _cont.OnConfirmPath;
                InputAction worldInteract = _worldMap.FindAction("Interact");
                worldInteract.performed -= _cont.OnInteract;
                _choosePath.Disable();
                InputAction worldPause = _worldMap.FindAction("Pause");
                worldPause.performed -= _cont.OnPause;

                InputAction next = _dialogueMap.FindAction("Next");
                next.performed -= NextDialogueBlock;
                InputAction dialoguePause = _dialogueMap.FindAction("Pause");
                dialoguePause.performed -= _cont.OnPause;
            }

            InputAction back = _uiMap.FindAction("Back");
            back.performed -= UIManager.Instance.OnUIBack;
        }

        private void FixedUpdate()
        {
            if (_cont)
            {
                if (_move.IsPressed()) _cont.OnMove(_move.ReadValue<Vector2>());
                if (_choosePath.IsPressed()) _cont.OnChoosePath(_choosePath.ReadValue<Vector2>());

                if (_look.enabled) _cont.OnLook(_look.ReadValue<Vector2>());
            }
        }

        void NextDialogueBlock(InputAction.CallbackContext _) => _dialogue.Next();

        #region Spell Modes

        void StartElementMode(InputAction.CallbackContext _)
        {
            _move.Disable();
            Time.timeScale = 0.75f;
            _cont.ResetInstructions();

            DOTween.To(() => 60, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 55, 1f);
        }

        void StopElementMode(InputAction.CallbackContext _)
        {
            _move.Enable();
            Time.timeScale = 1f;
            _cont.OnChooseElements();

            DOTween.To(() => 50, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 60, 0.1f);
        }

        void StartSpellMode(InputAction.CallbackContext _)
        {
            _move.Disable();
            Time.timeScale = 0.75f;
            _cont.ResetInstructions();

            DOTween.To(() => 60, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 55, 1f);
        }

        void StopSpellMode(InputAction.CallbackContext _)
        {
            _move.Enable();
            Time.timeScale = 1f;
            _cont.OnSpellLaunch();

            DOTween.To(() => 50, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 60, 0.1f);
        }

        #endregion

        #region Spells

        void RegisterSpellUp(InputAction.CallbackContext ctx)
        {
            if (_spell.IsPressed() || _element.IsPressed()) _cont.OnSpellInstruction(ESpellInstruction.Up);
        }
        void RegisterSpellDown(InputAction.CallbackContext ctx)
        {
            if (_spell.IsPressed() || _element.IsPressed()) _cont.OnSpellInstruction(ESpellInstruction.Down);
        }
        void RegisterSpellRight(InputAction.CallbackContext ctx)
        {
            if (_spell.IsPressed() || _element.IsPressed()) _cont.OnSpellInstruction(ESpellInstruction.Right);
        }
        void RegisterSpellLeft(InputAction.CallbackContext ctx)
        {
            if (_spell.IsPressed() || _element.IsPressed()) _cont.OnSpellInstruction(ESpellInstruction.Left);
        }

        #endregion
    }
}