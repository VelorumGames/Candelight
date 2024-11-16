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

        [SerializeField] UIManager _UIMan;

        PlayerParticlesManager _particles;

        #endregion

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _particles = GetComponent<PlayerParticlesManager>();

            _mage = FindObjectOfType<Mage>();

            DontDestroyOnLoad(gameObject);
        }

        private new void Start()
        {
            base.Start();

            _rb.maxLinearVelocity = _maxSpeed;            
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _UIMan = FindObjectOfType<UIManager>();

            OnNewInstruction += _UIMan.ShowNewInstruction;
            OnSpell += _UIMan.ShowValidSpell;
            OnElements += _UIMan.ShowValidElements;
            World.OnCandleChanged += _UIMan.RegisterCandle;

            _interaction = null;

            CanMove = true;
            
            if (!_currentNode && FindObjectOfType<WorldManager>() != null) _currentNode = FindObjectOfType<WorldManager>().CurrentNodeInfo.Node;
        }

        void OnSceneUnloaded(Scene scene)
        {
            UnloadInteraction();
            _pathShower.SetActive(false);

            OnNewInstruction -= _UIMan.ShowNewInstruction;
            OnSpell -= _UIMan.ShowValidSpell;
            OnElements -= _UIMan.ShowValidElements;
            World.OnCandleChanged -= _UIMan.RegisterCandle;
        }

        public override void RecieveDamage(float damage)
        {
            Debug.Log($"Jugador recibe {damage} de dano -> Se restan {damage * _candleFactor} puntos a la vela");

            float finalHealth = World.Candle - damage * _candleFactor;

            if (finalHealth <= 0 && _extraLives > 0) //Se revive al jugador si tiene vidas extras y va a morir
            {
                _extraLives--;
                finalHealth = 0.25f * World.MAX_CANDLE;
            }

            World.Candle = finalHealth;
        }

        #region Actions

        #region General

        public new void OnMove(Vector2 direction)
        {
            if (CanMove)
            {
                if (!_rb) _rb = GetComponent<Rigidbody>();
                Vector3 force = (_bookIsOpen ? 0.25f : 1f) * Time.deltaTime * 100f * _speed * _speedFactor * new Vector3(direction.x, 0f, direction.y);
                _rb.AddForce(force, ForceMode.Force);

                _particles.StartFootParticles();
            }
        }

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
            Debug.Log("Se carga interaccion de " + gameObject.name);
            _interaction = interaction;

            _selection.SetActive(true);
            _selection.transform.parent = obj;
            _selection.transform.position = obj.transform.position;
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

        #endregion

        #region Spells

        public void OnSpellInstruction(ESpellInstruction instr)
        {
            if (CanMove)
            {
                //Debug.Log("Se ha registrado la instruccion " + instr);
                _instructions.Add(instr);

                if (_bookIsOpen)
                {
                    if (_instrInBook) //Para que haya un delay y el jugador no spamee
                    {
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
                        Invoke("ResetBookInstructionTimer", 1f);
                    }
                }
                else
                {
                    if (OnNewInstruction != null) OnNewInstruction(instr);
                }
            }
        }

        public void ResetBookInstructionTimer() => _instrInBook = true;

        public void OnChooseElements()
        {
            if (_bookIsOpen) //Se registra un nuevo elemento
            {
                ARune.Activate(_instructions.ToArray());
                if (ARune.FindSpell(_instructions.ToArray(), out var rune)) _book.ShowResult(rune);
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
        }
        public void OnSpellLaunch()
        {
            if (_bookIsOpen) //Se registra una nueva forma
            {
                ARune.Activate(_instructions.ToArray());
                if (ARune.FindSpell(_instructions.ToArray(), out var rune)) _book.ShowResult(rune);
                else _book.ResetText();
            }
            else //Se lanza un hechizo
            {
                if (_mage.GetActiveElements().Count > 0)
                {
                    string str = "";
                    foreach (ESpellInstruction i in _instructions) str += i.ToString();
                    Debug.Log("Se lanza hechizo: " + str);
                    if (ARune.FindSpell(_instructions.ToArray(), out var spell))
                    {
                        Debug.Log("Hechizo encontrado!!: " + spell.Name);
                        AShapeRune shapeSpell = spell as AShapeRune;
                        if (shapeSpell != null)
                        {
                            shapeSpell.ThrowSpell();
                            if (OnSpell != null) OnSpell(shapeSpell);
                        }
                        else if (OnSpell != null) OnSpell(null); //Si no encuentra hechizo valido
                    }
                    else if (OnSpell != null) OnSpell(null); //Si no encuentra hechizo valido
                    ResetInstructions();
                }
            }
        }

        public void ResetInstructions() => _instructions.Clear();

        public void OnBook(InputAction.CallbackContext _)
        {
            if (_book)
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
                //Debug.Log($"Para el current node hay {_currentNode.ConnectedNodes.Count} nodos conectados");
                foreach (var node in _currentNode.ConnectedNodes)
                {
                    if (node != _currentNode.gameObject && node.GetComponent<NodeManager>().GetNodeData().State != ENodeState.Undiscovered)
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

        #endregion

        private void FixedUpdate()
        {
            _rb.maxLinearVelocity = _maxSpeed;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}