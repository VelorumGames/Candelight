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
using Music;

namespace Controls
{
    public enum EControlMap
    {
        None,
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
        InputAction _scroll;
        

        MusicManager _music;

        bool _isInSpellMode;
        bool _elementMode;
        bool _shapeMode;

        public event System.Action OnStartElementMode;
        public event System.Action OnExitElementMode;
        public event System.Action OnStartShapeMode;
        public event System.Action OnExitShapeMode;

        UIManager _ui;

        private void OnEnable()
        {
            if (!GameSettings.LoadedControls)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;

                OnStartElementMode += _cont.RegisterSpellMode;
                OnStartElementMode += _cont.ResetInstructions;
                OnStartElementMode += _cont.StartSpellMove;
                OnStartElementMode += _music.EnterSpellModeMusic;

                OnExitElementMode += _cont.OnChooseElements;
                OnExitElementMode += _music.ExitSpellModeMusic;
                OnExitElementMode += _cont.ExitSpellMode;

                OnStartShapeMode += _cont.RegisterSpellMode;
                OnStartShapeMode += _cont.ResetInstructions;
                OnStartShapeMode += _cont.StartSpellMove;
                OnStartShapeMode += _cont.ShowOrientationArrow;
                OnStartShapeMode += _music.EnterSpellModeMusic;

                OnExitShapeMode += _cont.OnSpellLaunch;
                OnExitShapeMode += _music.ExitSpellModeMusic;
                OnExitShapeMode += _cont.ExitSpellMode;
                OnExitShapeMode += _cont.HideOrientationArrow;

                GameSettings.LoadedControls = true;
            }
        }

        private void Awake()
        {
            //if (Instance != null) Destroy(gameObject);
            //else Instance = this;
            _music = FindObjectOfType<MusicManager>();

            DontDestroyOnLoad(gameObject);

            if (!GameSettings.LoadedControls) InitializeControls();

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
            _ui = FindObjectOfType<UIManager>();
        }

        void OnSceneUnloaded(Scene scene)
        {

        }

        void InitializeControls()
        {
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
            InputAction throwLast = _levelMap.FindAction("ThrowLastSpell");
            throwLast.performed += _cont.OnLastSpellLaunch;

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

            //UI
            _uiMap = Input.FindActionMap("UI");
            InputAction back = _uiMap.FindAction("Back");
            back.performed += UIBack;
            _scroll = _uiMap.FindAction("Scroll");
            _scroll.performed += UIScroll;
        }

        void UnloadControls()
        {
            _cont = FindObjectOfType<PlayerController>();

            //Level
            _levelMap = Input.FindActionMap("Level");

            _move = _levelMap.FindAction("Move");
            //_move.canceled += _cont.OnStopMove;
            InputAction interact = _levelMap.FindAction("Interact");
            interact.performed -= _cont.OnInteract;
            _element = _levelMap.FindAction("Element");
            _element.performed -= StartElementMode;
            _element.canceled -= StopElementMode;
            _spell = _levelMap.FindAction("Spell");
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
            _look = _levelMap.FindAction("Look");
            InputAction inventory = _levelMap.FindAction("Inventory");
            inventory.performed -= _cont.OnInventory;
            InputAction throwLast = _levelMap.FindAction("ThrowLastSpell");
            throwLast.performed -= _cont.OnLastSpellLaunch;

            //World
            _worldMap = Input.FindActionMap("World");

            _choosePath = _worldMap.FindAction("ChoosePath");
            InputAction confirmPath = _worldMap.FindAction("ConfirmPath");
            confirmPath.performed -= _cont.OnConfirmPath;
            InputAction worldInteract = _worldMap.FindAction("Interact");
            worldInteract.performed -= _cont.OnInteract;
            InputAction worldPause = _worldMap.FindAction("Pause");
            worldPause.performed -= _cont.OnPause;
            InputAction worldInventory = _worldMap.FindAction("Inventory");
            worldInventory.performed -= _cont.OnInventory;

            //Dialogue
            _dialogueMap = Input.FindActionMap("Dialogue");

            InputAction next = _dialogueMap.FindAction("Next");
            next.performed -= NextDialogueBlock;
            InputAction dialoguePause = _dialogueMap.FindAction("Pause");
            dialoguePause.performed -= _cont.OnPause;

            //Intro
            _introMap = Input.FindActionMap("Intro");

            _introMove = _introMap.FindAction("Move");
            InputAction introPause = _introMap.FindAction("Pause");
            introPause.performed -= _cont.OnPause;
            _introLook = _introMap.FindAction("Look");

            //UI
            _uiMap = Input.FindActionMap("UI");
            InputAction back = _uiMap.FindAction("Back");
            back.performed -= UIBack;
            _scroll = _uiMap.FindAction("Scroll");
            _scroll.performed -= UIScroll;
        }

        public void LoadControls(EControlMap map)
        {
            //Debug.Log("Mapa previo: " + _currentMap + $" y se colocara {map}");
            _prevControls = _currentMap;
            if (_currentMap != null) _currentMap.Disable();

            if (_ui == null) _ui = FindObjectOfType<UIManager>();

            switch (map)
            {
                case EControlMap.Level:
                    _currentMap = _levelMap;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;

                    if (SceneManager.GetActiveScene().name == "CalmScene") _ui.ShowUIMode(EUIMode.Calm);
                    else _ui.ShowUIMode(EUIMode.Explore);
                    break;
                case EControlMap.World:
                    _currentMap = _worldMap;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;

                    _ui.ShowUIMode(EUIMode.Explore);
                    break;
                case EControlMap.Dialogue:
                    _currentMap = _dialogueMap;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                    //Cursor.lockState = CursorLockMode.Locked;

                    _ui.ShowUIMode(EUIMode.Dialogue);
                    break;
                case EControlMap.UI:
                    _currentMap = _uiMap;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
                case EControlMap.Intro:
                    _currentMap = _introMap;

                    Cursor.visible = false;
                    //Cursor.lockState = CursorLockMode.Locked;
                    break;
                default:
                    _currentMap = null;
                    break;
            }
            //Debug.Log("Mapa colocado: " + _currentMap);

            if (Application.isMobilePlatform)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }

            try
            {
                _currentMap.Enable();
                CurrentControls = _currentMap.name;
            }
            catch(System.Exception e)
            {
                Debug.LogWarning("ERROR: " + e);
                _currentMap = _worldMap;
            }
        }

        public void LoadControls(InputActionMap map)
        {
            //Debug.Log("Mapa previo: " + _currentMap);
            _prevControls = _currentMap;
            if (_currentMap != null) _currentMap.Disable();

            if (map == _levelMap)
            {
                _currentMap = _levelMap;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                if (SceneManager.GetActiveScene().name == "CalmScene") _ui.ShowUIMode(EUIMode.Calm);
                else _ui.ShowUIMode(EUIMode.Explore);
            }
            else if (map == _worldMap)
            {
                _currentMap = _worldMap;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                _ui.ShowUIMode(EUIMode.Explore);
            }
            else if (map == _dialogueMap)
            {
                _currentMap = _dialogueMap;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                _ui.ShowUIMode(EUIMode.Dialogue);
            }
            else if (map == _uiMap)
            {
                _currentMap = _uiMap;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else if (map == _introMap)
            {
                _currentMap = _introMap;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else Debug.LogWarning("ERROR: No se han cargado bien los controles");
            //Debug.Log("Mapa colocado: " + _currentMap);
            _currentMap.Enable();
            CurrentControls = _currentMap.name;
        }

        public void LoadPreviousControls()
        {
            if (_prevControls != null)
            {
                LoadControls(_prevControls);
            }
        }

        void UIBack(InputAction.CallbackContext _)
        {
            if (_ui != null) _ui.OnUIBack(_);
        }

        void UIScroll(InputAction.CallbackContext ctx)
        {
            if (_ui != null) _ui.Scroll(ctx.ReadValue<Vector2>().y);
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

        public void StartElementMode(InputAction.CallbackContext _)
        {
            if (!_shapeMode && SceneManager.GetActiveScene().name != "CalmScene" && SceneManager.GetActiveScene().name != "NodeEndScene")
            {
                if (OnStartElementMode != null) OnStartElementMode();
                _elementMode = true;
                _isInSpellMode = true;
                _move.Disable();
            }
        }

        public void StopElementMode(InputAction.CallbackContext _)
        {
            if (!_shapeMode && SceneManager.GetActiveScene().name != "CalmScene" && SceneManager.GetActiveScene().name != "NodeEndScene")
            {
                if (OnExitElementMode != null) OnExitElementMode();
                _elementMode = false;
                _isInSpellMode = false;
                _move.Enable();
            }
        }

        public void StartSpellMode(InputAction.CallbackContext _)
        {
            if (!_elementMode && SceneManager.GetActiveScene().name != "CalmScene" && SceneManager.GetActiveScene().name != "NodeEndScene")
            {
                if (OnStartShapeMode != null) OnStartShapeMode();

                _shapeMode = true;
                _isInSpellMode = true;
                _move.Disable();
            }
        }

        public void StopSpellMode(InputAction.CallbackContext _)
        {
            if (!_elementMode && SceneManager.GetActiveScene().name != "CalmScene" && SceneManager.GetActiveScene().name != "NodeEndScene")
            {
                if (OnExitShapeMode != null) OnExitShapeMode();
                _shapeMode = false;
                _isInSpellMode = false;
                _move.Enable();
            }
        }

        #endregion

        #region Spells

        public void RegisterSpellUp(InputAction.CallbackContext ctx)
        {
            if (_shapeMode || _elementMode) _cont.OnSpellInstruction(ESpellInstruction.Up);
        }
        public void RegisterSpellDown(InputAction.CallbackContext ctx)
        {
            if (_shapeMode || _elementMode) _cont.OnSpellInstruction(ESpellInstruction.Down);
        }
        public void RegisterSpellRight(InputAction.CallbackContext ctx)
        {
            if (_shapeMode || _elementMode) _cont.OnSpellInstruction(ESpellInstruction.Right);
        }
        public void RegisterSpellLeft(InputAction.CallbackContext ctx)
        {
            if (_shapeMode || _elementMode) _cont.OnSpellInstruction(ESpellInstruction.Left);
        }

        #endregion

        public float GetScrollData() => _scroll.ReadValue<Vector2>().y;

        public bool IsInSpellMode() => _isInSpellMode;

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            OnStartElementMode -= _cont.RegisterSpellMode;
            OnStartElementMode -= _cont.ResetInstructions;
            OnStartElementMode -= _cont.StartSpellMove;
            OnStartElementMode -= _music.EnterSpellModeMusic;

            OnExitElementMode -= _cont.OnChooseElements;
            OnExitElementMode -= _music.ExitSpellModeMusic;
            OnExitElementMode -= _cont.ExitSpellMode;

            OnStartShapeMode -= _cont.RegisterSpellMode;
            OnStartShapeMode -= _cont.ResetInstructions;
            OnStartShapeMode -= _cont.StartSpellMove;
            OnStartShapeMode -= _cont.ShowOrientationArrow;
            OnStartShapeMode -= _music.EnterSpellModeMusic;

            OnExitShapeMode -= _cont.OnSpellLaunch;
            OnExitShapeMode -= _music.ExitSpellModeMusic;
            OnExitShapeMode -= _cont.ExitSpellMode;
            OnExitShapeMode -= _cont.HideOrientationArrow;

            if (_cont) UnloadControls();
        }
    }
}