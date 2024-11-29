using Animations;
using Cameras;
using Cinemachine;
using Controls;
using DG.Tweening;
using Hechizos;
using Hechizos.DeForma;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using World;

namespace Player
{
    public class PlayerController : AController
    {
        #region Variables

        [SerializeField] float _maxSpeed;
        float _speedFactor = 1f;

        [SerializeField] GameObject _pathShower;
        Coroutine _pathShowRotation;
        [SerializeField] GameObject _pathChooser;
        NodeManager m_current;
        NodeManager _currentNode
        {
            get => m_current;
            set
            {
                m_current = value;
                if (UIManager.Instance != null && m_current != null) UIManager.Instance.ActualNodeName = m_current.gameObject.name;
            }
        }
        Transform m_next;
        Transform _nextNode
        {
            get => m_next;
            set
            {
                if (m_next != value)
                {
                    m_next = value;
                    if (UIManager.Instance != null && m_next != null) UIManager.Instance.NextNodeName = _nextNode.gameObject.name;

                    Debug.Log("Nuevo nodo");
                    if (_pathShowRotation != null) StopCoroutine(_pathShowRotation);
                    _pathShowRotation = StartCoroutine(RotatePathShower());
                }
            }
        }
        [SerializeField] GameObject _selection;

        List<ESpellInstruction> _instructions = new List<ESpellInstruction>();
        Mage _mage;

        float _candleFactor = 1f;
        int _extraLives;

        Action _interaction;

        bool _bookIsOpen;
        bool _instrInBook = true;
        [SerializeField] BookManager _book;

        public event Action<ESpellInstruction> OnNewInstruction;
        public event Action<AShapeRune> OnSpell;
        public event Action<AElementalRune[]> OnElements;
        public event Action<float> OnRevive;

        UIManager _UIMan;
        InputManager _input;

        SacrificadoAnimation _anim;

        PlayerParticlesManager _particles;
        [SerializeField] CinemachineVirtualCamera _fpCam;
        [SerializeField] bool _isFirstPerson;
        CameraManager _camMan;

        bool _inCombat;
        float _spellThrowDelay = 0.7f;
        bool _canSpellThrow = true;

        bool _invicible;
        float _iFrameDuration = 0.5f;

        Vector2 _oldDirection;

        AShapeRune _lastSpell;
        float _lastSpellDuration = 5f;
        bool _canLastSpell;
        Vector3 force;

        [Space(10)]
        [Header("FIRST PERSON")]
        float _fpOrientation;
        float _mouseSens = 0.2f;

        #endregion

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            World.OnPlayerDeath += Death;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _particles = GetComponent<PlayerParticlesManager>();
            _anim = GetComponentInChildren<SacrificadoAnimation>();

            _mage = FindObjectOfType<Mage>();
            _input = FindObjectOfType<InputManager>();

            DontDestroyOnLoad(gameObject);

            force = new Vector3();
        }

        private new void Start()
        {
            base.Start();

            _rb.maxLinearVelocity = _maxSpeed;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _UIMan = FindObjectOfType<UIManager>();
            _camMan = FindObjectOfType<CameraManager>();
            if (_camMan != null) _camMan.AddCamera(_fpCam);

            _interaction = null;
            CanMove = true;
            _isFirstPerson = false;
            
            if (_currentNode != null && FindObjectOfType<WorldManager>() != null) _currentNode = FindObjectOfType<WorldManager>().CurrentNodeInfo.Node;

            if (scene.name == "WorldScene")
            {
                _rb.useGravity = true;
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        void OnSceneUnloaded(Scene scene)
        {
            UnloadInteraction();
            _pathShower.SetActive(false);
        }

        public NodeManager GetCurrentNode() => _currentNode;
        public void SetCurrentNode(NodeManager node) => _currentNode = node;

        public override void RecieveDamage(float damage)
        {
            if (!_invicible)
            {
                _invicible = true;
                Invoke("ManageIFrames", _iFrameDuration);

                float finalDamage = damage * _candleFactor;

                Debug.Log($"Jugador recibe {damage} de dano -> Se restan {finalDamage} puntos a la vela");

                float finalHealth = World.Candle - finalDamage;

                if (finalHealth <= 0 && _extraLives > 0) //Se revive al jugador si tiene vidas extras y va a morir
                {
                    _extraLives--;
                    finalHealth = 0.25f * World.MAX_CANDLE;
                }

                //TODO. Esto cambiara para la gold.
                if (finalHealth <= 0) //Morira
                {
                    CanMove = false;
                    _anim.ChangeToDeath();
                }

                CallDamageEvent(finalDamage, Mathf.Clamp01(finalHealth / World.MAX_CANDLE));

                World.Candle = finalHealth;
            }
        }

        public void ManageIFrames()
        {
            _invicible = false;
        }

        public void RegisterCombat() => _inCombat = true;
        public void FinishCombat() => _inCombat = false;

        public void Revive()
        {
            CanMove = true;

            Debug.Log("SE REVIVE AL JUGADOR");
            World.Candle = World.MAX_CANDLE * 0.5f;

            _anim.ChangeToLife();

            if (OnRevive != null) OnRevive(World.Candle);
        }

        void Death()
        {
            CanMove = false;
        }

        #region Actions

        #region General

        public new void OnMove(Vector2 direction)
        {
            if (CanMove)
            {
                if (!_rb) _rb = GetComponent<Rigidbody>();
                force = (_bookIsOpen || _isFirstPerson ? 0.25f : 1f) * Time.deltaTime * 100f * _speed * _speedFactor * new Vector3(direction.x, 0f, direction.y);
                _rb.AddForce(force, ForceMode.Force);

                _particles.StartFootParticles();

                if (_inCombat && direction != _oldDirection) OnCombatMoveBoost(force);

                _oldDirection = direction;
            }
        }

        public void StartSpellMove()
        {

            if (_rb.velocity.magnitude > 4f) StartCoroutine(OnSpellMove());
        }

        IEnumerator OnSpellMove()
        {
            yield return new WaitForEndOfFrame();

            while (_input.IsInSpellMode())
            {
                force *= 0.975f;
                _rb.AddForce(force * Time.deltaTime * 100f, ForceMode.Force);
                _particles.StartFootParticles();

                yield return null;
            }
        }

        public void OnCombatMoveBoost(Vector3 force)
        {
            _maxSpeed *= 1.1f;
            _rb.AddForce(force, ForceMode.Impulse);
            Invoke("ResetMaxVelocity", 0.5f);
        }

        public void ResetMaxVelocity() => _maxSpeed /= 1.1f;

        public void OnStopMove()
        {
            _particles.StopFootParticles();
        }

        public void OnInteract(InputAction.CallbackContext _)
        {
            if (_interaction != null && _selection != null)
            {
                _selection.transform.parent = transform;
                _selection.transform.localPosition = new Vector3();
                _selection.SetActive(false);
                _interaction();
            }
        }

        public void LoadInteraction(Action interaction, Transform obj)
        {
            if (interaction != _interaction)
            {
                Debug.Log("Se carga interaccion de " + gameObject.name);
                _interaction = interaction;

                _selection.SetActive(true);
                _selection.transform.parent = obj;
                _selection.transform.position = obj.transform.position;
            }
        }

        public void UnloadInteraction()
        {
            Debug.Log("Se descarga interaccion de " + gameObject.name);
            _interaction = null;

            _selection.transform.parent = transform;
            _selection.transform.localPosition = new Vector3();
            _selection.SetActive(false);
        }

        public void OnLook(Vector2 mousePos)
        {
            if (CanMove)
            {
                Orientation = new Vector3(mousePos.x / Screen.width - 0.5f, 0f, mousePos.y / Screen.height - 0.5f).normalized;
                Debug.DrawRay(transform.position, 10f * Orientation, Color.red);
            }
        }

        public void OnFirstPersonLook(Vector2 delta)
        {
            if (_isFirstPerson)
            {
                _fpOrientation = Mathf.Clamp(_fpOrientation + delta[1] * _mouseSens * Time.deltaTime * 10f, -30f, 30f);
                _camMan.GetActiveCam().transform.rotation = Quaternion.Euler(-_fpOrientation, 0f, 0f);
            }
        }

        #endregion

        #region Spells

        public void OnSpellInstruction(ESpellInstruction instr)
        {
            if (CanMove && SceneManager.GetActiveScene().name != "CalmScene")
            {
                //Debug.Log("Se ha registrado la instruccion " + instr);
                if (_bookIsOpen)
                {
                    if (_instrInBook) //Para que haya un delay y el jugador no spamee
                    {
                        _instructions.Add(instr);
                        switch (instr)
                        {
                            case ESpellInstruction.Up:
                                _book.AddNewString("W");
                                break;
                            case ESpellInstruction.Down:
                                _book.AddNewString("S");
                                break;
                            case ESpellInstruction.Right:
                                _book.AddNewString("D");
                                break;
                            case ESpellInstruction.Left:
                                _book.AddNewString("A");
                                break;
                        }

                        _instrInBook = false;
                        _UIMan.VignetteFeedback(0.25f);
                        Invoke("ResetBookInstructionTimer", 0.25f);
                    }
                }
                else
                {
                    _instructions.Add(instr);
                    if (OnNewInstruction != null) OnNewInstruction(instr);
                }
            }
        }

        public void ResetBookInstructionTimer() => _instrInBook = true;

        public void OnChooseElements()
        {
            if (_bookIsOpen) //Se registra un nuevo elemento
            {
                ARune.Activate(_instructions.ToArray(), out var rune);
                if (rune != null) _book.ShowResult(rune);
                else _book.ResetText();
            }
            else //Se activa un elemento(s)
            {
                if (ARune.FindElements(_instructions.ToArray(), out var elements))
                {
                    Debug.Log("Se encuentran elementos que aplicar: " + elements);
                    _mage.SetActiveElements(elements);
                    if (OnElements != null) OnElements(elements);
                }
                else if (OnElements != null) OnElements(null); //Si no encuentra elemento valido
            }

            _UIMan.ManageAuxiliarRuneReset();
        }
        public void OnSpellLaunch()
        {
            if (_bookIsOpen) //Se registra una nueva forma
            {
                ARune.Activate(_instructions.ToArray(), out var rune);
                if (rune != null) _book.ShowResult(rune);
                else _book.ResetText();
            }
            else //Se lanza un hechizo
            {
                if (_mage.GetActiveElements().Count > 0)
                {
                    string str = "";
                    foreach (ESpellInstruction i in _instructions) str += i.ToString();
                    //Debug.Log("Se lanza hechizo: " + str);
                    if (ARune.FindSpell(_instructions.ToArray(), out var spell))
                    {
                        Debug.Log("Hechizo encontrado!!: " + spell.Name);
                        AShapeRune shapeSpell = spell as AShapeRune;
                        if (shapeSpell != null)
                        {
                            if (!(shapeSpell is ExplosionRune))
                            {
                                _canLastSpell = true;
                                Invoke("ResetLastSpellTimer", _lastSpellDuration);
                            }

                            ThrowSpell(shapeSpell);
                        }
                        else if (OnSpell != null) OnSpell(null); //Si no encuentra hechizo valido
                    }
                    else if (OnSpell != null) OnSpell(null); //Si no encuentra hechizo valido
                    ResetInstructions();
                }
            }

            _UIMan.ManageAuxiliarRuneReset();
        }

        public void OnLastSpellLaunch(InputAction.CallbackContext _)
        {
            if (_canLastSpell && CanMove && SceneManager.GetActiveScene().name != "CalmScene" && _lastSpell != null)
            {
                ThrowSpell(_lastSpell);
            }
        }

        public void ResetLastSpellTimer() => _canLastSpell = false;

        void ThrowSpell(AShapeRune shapeSpell)
        {
            if (_canSpellThrow && !_bookIsOpen && !_isFirstPerson)
            {
                _lastSpell = shapeSpell;

                shapeSpell.ThrowSpell();
                if (OnSpell != null) OnSpell(shapeSpell);

                if (shapeSpell is MeleeRune) _anim.ChangeToMelee();

                _UIMan.ResetCanShoot();
                _UIMan.VignetteFeedback(_spellThrowDelay);

                _canSpellThrow = false;
                Invoke("ResetSpellThrowDelay", _spellThrowDelay);
            }
        }

        public void ResetSpellThrowDelay()
        {
            _UIMan.ShowCanShoot();
            _canSpellThrow = true;
        }

        public void ResetInstructions()
        {
            _instructions.Clear();
        }

        public void OnBook(InputAction.CallbackContext _)
        {
            if (!_isFirstPerson && _book && SceneManager.GetActiveScene().name != "CalmScene")
            {
                if (_bookIsOpen)
                {
                    _book.gameObject.SetActive(false);
                    _bookIsOpen = false;
                }
                else
                {
                    _book.gameObject.SetActive(true);
                    _bookIsOpen = true;
                }
            }
        }

        #endregion

        public void OnChoosePath(Vector2 direction)
        {
            if (_currentNode != null)
            {
                //Movemos un gameobject invisible
                _pathChooser.transform.localPosition += new Vector3(direction.x, 0f, direction.y);
                _pathChooser.transform.localPosition = Vector3.ClampMagnitude(_pathChooser.transform.localPosition, 7f);

                //Comparamos con que nodo conectado al actual esta mas cerca y consideramos ese como la decision del jugador
                float minDist = 9999f;
                GameObject closest = null;

                foreach (var node in _currentNode.ConnectedNodes)
                {
                    if ((node != _currentNode.gameObject && node.GetComponent<NodeManager>().GetNodeData().State != ENodeState.Sin_Descubrir && _currentNode.GetComponent<NodeManager>().GetNodeData().State == ENodeState.Completado) ||
                        (node != _currentNode.gameObject && node.GetComponent<NodeManager>().GetNodeData().State == ENodeState.Completado && _currentNode.GetComponent<NodeManager>().GetNodeData().State == ENodeState.Explorado) )
                    {
                        float dist = Vector3.Distance(_pathChooser.transform.position, node.transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            closest = node;
                        }
                    }
                }
                if (closest != null) _nextNode = closest.transform;
            }
        }

        public void OnConfirmPath(InputAction.CallbackContext _)
        {
            if (_rb && _rb.velocity.magnitude < 0.1f)
            {
                _pathShower.SetActive(false);
                StartCoroutine(MovePlayerTowardsNode(_nextNode));
            }
        }

        public void OnPause(InputAction.CallbackContext _)
        {
            UIManager.Instance.LoadUIWindow(UIManager.Instance.PauseMenu);
        }

        public void OnInventory(InputAction.CallbackContext _)
        {
            UIManager.Instance.LoadUIWindow(UIManager.Instance.InventoryUI, "i");
        }

        #endregion

        IEnumerator MovePlayerTowardsNode(Transform target)
        {
            if (_nextNode != null)
            {
                Vector3 direction = new Vector3((_nextNode.position - transform.position).x, 0f, (_nextNode.position - transform.position).z).normalized;
                while (Vector3.Distance(transform.position, target.position) > 1.5f)
                {
                    OnMove(new Vector2(direction.x, direction.z));
                    yield return null;
                }
                _currentNode = target.GetComponent<NodeManager>();
                yield return null;
            }
        }

        IEnumerator RotatePathShower()
        {
            _pathShower.SetActive(true);

            Vector3 dir = _nextNode.transform.position - transform.position;

            int rotWise = Vector3.SignedAngle(_pathShower.transform.forward, dir, Vector3.up) < 0 ? -1 : 1; //Para saber en que direccion debe rotar

            Quaternion endRot = Quaternion.Euler(new Vector3(0f, Vector3.SignedAngle(transform.forward, dir, Vector3.up), 0f));
            Quaternion currentRot = _pathShower.transform.rotation;

            while (Mathf.Abs(endRot.eulerAngles.y - currentRot.eulerAngles.y) > 4f)
            {
                _pathShower.transform.RotateAround(transform.position, Vector3.up, rotWise * 400f * Time.deltaTime);
                currentRot = _pathShower.transform.rotation;
                yield return null;
            }
        }
        public void ChangeToFirstPerson()
        {
            if (!_isFirstPerson)
            {
                _camMan.SetActiveCamera(_fpCam, 1.5f);

                _isFirstPerson = true;
            }
        }

        public void ReturnToThirdPerson()
        {
            if (_isFirstPerson)
            {
                _camMan.SetActiveCamera(_camMan.InitialCam, 1f);

                _isFirstPerson = false;
            }
        }


        #region Item Modifiers

        public void SetCandleFactor(float f) => _candleFactor = f;
        public void AddCandleFactor(float f) => _candleFactor += f;
        public void RemoveCandleFactor(float f) => _candleFactor -= f;
        public float GetCandleFactor() => _candleFactor;

        public void SetSpeedFactor(float s) => _speedFactor = s;
        public void AddSpeedFactor(float s) => _speedFactor += s;
        public void RemoveSpeedFactor(float s) => _speedFactor -= s;
        public float GetSpeedFactor() => _speedFactor;

        public void AddExtraLife(int n) => _extraLives += n;

        public float GetLastSpellDelay() => _lastSpellDuration;

        #endregion

        private void FixedUpdate()
        {
            _rb.maxLinearVelocity = _maxSpeed;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            World.OnPlayerDeath -= Death;
        }
    }
}