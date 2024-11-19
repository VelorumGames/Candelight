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
using UnityEngine.SceneManagement;
using BehaviourAPI.Core.Actions;

namespace Controls
{
    public enum EControlMap
    {
        Level,
        World,
        Dialogue,
        UI,
        Intro
    }

    public enum ESpellInstruction
    {
        Up, Down, Right, Left
    }

    public class InputManager : MonoBehaviour
    {
        //public static InputManager Instance;

        public InputActionAsset Input;
        [SerializeField] EControlMap _initialControls;
        public string CurrentControls;
        InputActionMap _prevControls;
        PlayerController _cont;

        InputActionMap _currentMap;
        InputActionMap _worldMap;
        InputActionMap _levelMap;
        InputActionMap _dialogueMap;
        InputActionMap _uiMap;
        InputActionMap _introMap;

        DialogueUI _dialogue;

        InputAction _move;
        InputAction _introMove;
        InputAction _choosePath;
        InputAction _element;
        InputAction _spell;
        InputAction _look;
        InputAction _introLook;

        CameraManager _camMan;
        

        float _spellTimeScale = 0.75f;

        public event System.Action OnStartElementMode;
        public event System.Action OnExitElementMode;
        public event System.Action OnStartShapeMode;
        public event System.Action OnExitShapeMode;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Awake()
        {
            //if (Instance != null) Destroy(gameObject);
            //else Instance = this;
            

            DontDestroyOnLoad(gameObject);

            InitializeControls();

            DOTween.Init();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            LoadControls(_initialControls);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _dialogue = FindObjectOfType<DialogueUI>();
            _camMan = FindObjectOfType<CameraManager>();

            //UI
            _uiMap = Input.FindActionMap("UI");

            InputAction back = _uiMap.FindAction("Back");
            back.performed += FindObjectOfType<UIManager>().OnUIBack;
        }

        void OnSceneUnloaded(Scene scene)
        {
            
        }

        void InitializeControls()
        {
            Debug.Log("Se inicializan los controles");
            _cont = FindObjectOfType<PlayerController>();

            //Level
            _levelMap = Input.FindActionMap("Level");

            _move = _levelMap.FindAction("Move");
            //_move.canceled += _cont.OnStopMove;
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
            InputAction inventory = _levelMap.FindAction("Inventory");
            inventory.performed += _cont.OnInventory;

            //World
            _worldMap = Input.FindActionMap("World");

            _choosePath = _worldMap.FindAction("ChoosePath");
            InputAction confirmPath = _worldMap.FindAction("ConfirmPath");
            confirmPath.performed += _cont.OnConfirmPath;
            InputAction worldInteract = _worldMap.FindAction("Interact");
            worldInteract.performed += _cont.OnInteract;
            InputAction worldPause = _worldMap.FindAction("Pause");
            worldPause.performed += _cont.OnPause;
            InputAction worldInventory = _worldMap.FindAction("Inventory");
            worldInventory.performed += _cont.OnInventory;

            //Dialogue
            _dialogueMap = Input.FindActionMap("Dialogue");

            InputAction next = _dialogueMap.FindAction("Next");
            next.performed += NextDialogueBlock;
            InputAction dialoguePause = _dialogueMap.FindAction("Pause");
            dialoguePause.performed += _cont.OnPause;

            //Intro
            _introMap = Input.FindActionMap("Intro");

            _introMove = _introMap.FindAction("Move");
            InputAction introPause = _introMap.FindAction("Pause");
            introPause.performed += _cont.OnPause;
            _introLook = _introMap.FindAction("Look");
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

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
                case EControlMap.World:
                    _currentMap = _worldMap;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
                case EControlMap.Dialogue:
                    _currentMap = _dialogueMap;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case EControlMap.UI:
                    _currentMap = _uiMap;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
                case EControlMap.Intro:
                    _currentMap = _introMap;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
            }
            //Debug.Log("Mapa colocado: " + _currentMap);
            _currentMap.Enable();
            CurrentControls = _currentMap.name;
        }

        public void LoadPreviousControls()
        {
            if (_prevControls != null)
            {
                Debug.Log("Controles antiguos: " + _currentMap.name);
                _currentMap.Disable();
                _currentMap = _prevControls;
                Debug.Log("Controles actuales: " + _currentMap.name);
                _currentMap.Enable();
                CurrentControls = _currentMap.name;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void FixedUpdate()
        {
            if (_cont)
            {
                if (_introMove.IsPressed()) _cont.OnMove(_introMove.ReadValue<Vector2>());

                if (_move.IsPressed()) _cont.OnMove(_move.ReadValue<Vector2>());
                else _cont.OnStopMove();

                if (_choosePath.IsPressed()) _cont.OnChoosePath(_choosePath.ReadValue<Vector2>());

                if (_look.enabled) _cont.OnLook(_look.ReadValue<Vector2>());

                if (_introLook.enabled) _cont.OnFirstPersonLook(_introLook.ReadValue<Vector2>());
            }
        }

        void NextDialogueBlock(InputAction.CallbackContext _)
        {
            if (_dialogue) _dialogue.Next();
        }

        #region Spell Modes

        void StartElementMode(InputAction.CallbackContext _)
        {
            if (OnStartElementMode != null) OnStartElementMode();

            _move.Disable();
            //Time.timeScale = _spellTimeScale;
            //DOTween.To(() => Time.timeScale, x => Time.timeScale = x, _spellTimeScale, 0.2f);
            //_cont.ResetInstructions();

            //if (_camMan != null) _camMan.EnterSpellMode();
        }

        void StopElementMode(InputAction.CallbackContext _)
        {
            if (OnExitElementMode != null) OnExitElementMode();

            _move.Enable();
            //Time.timeScale = 1f;
            //DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.1f);
            //_cont.OnChooseElements();

            //if (_camMan != null) _camMan.ExitSpellMode();
        }

        void StartSpellMode(InputAction.CallbackContext _)
        {
            if (OnStartShapeMode != null) OnStartShapeMode();

            _move.Disable();
            //Time.timeScale = _spellTimeScale;
            //DOTween.To(() => Time.timeScale, x => Time.timeScale = x, _spellTimeScale, 0.2f);
            //_cont.ResetInstructions();

            //if (_camMan != null) _camMan.EnterSpellMode();
        }

        void StopSpellMode(InputAction.CallbackContext _)
        {
            if (OnExitShapeMode != null) OnExitShapeMode();

            _move.Enable();
            //Time.timeScale = 1f;
            //DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.1f);
            //_cont.OnSpellLaunch();

            //if (_camMan != null) _camMan.ExitSpellMode();
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

        public void SetSpellTimeScale(float t) => _spellTimeScale = t;
        public float GetSpellTimeScale() => _spellTimeScale;
    }
}