using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using World;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _rb;
        [SerializeField] float _speed;
        [SerializeField] float _maxSpeed;

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
                }
            }
        }
        [SerializeField] GameObject _selection;
        Vector3 _oSelectionPos;

        Action _interaction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rb.maxLinearVelocity = _maxSpeed;
            _interaction = null;

            _oSelectionPos = _selection.transform.position;

            if (!_currentNode && WorldManager.Instance) _currentNode = WorldManager.Instance.CurrentNodeInfo.Node;
        }

        public void OnMove(Vector2 direction)
        {
            Vector3 force = Time.deltaTime * 100f * _speed * new Vector3(direction.x, 0f, direction.y);
            _rb.AddForce(force, ForceMode.Force);
        }

        public void OnInteract(InputAction.CallbackContext _)
        {
            if (_interaction != null) _interaction();
        }

        public void OnChoosePath(Vector2 direction)
        {
            //Movemos un gameobject invisible
            _pathChooser.transform.localPosition += new Vector3(direction.x, 0f, direction.y);
            _pathChooser.transform.localPosition = Vector3.ClampMagnitude(_pathChooser.transform.localPosition, 7f);

            //Comparamos con que nodo conectado al actual esta mas cerca y consideramos ese como la decision del jugador
            float minDist = 9999f;
            GameObject closest = null;
            foreach (var node in _currentNode.ConnectedNodes)
            {
                if (node != _currentNode.gameObject /*&& node.GetComponent<NodeManager>().GetNodeData().State != ENodeState.Undiscovered*/) //Se queda comentado mientras queramos debuggear
                {
                    float dist = Vector3.Distance(_pathChooser.transform.position, node.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = node;
                    }
                }
            }
            _nextNode = closest.transform;

        }

        public void OnConfirmPath(InputAction.CallbackContext _)
        {
            if (_rb.velocity.magnitude < 0.1f) StartCoroutine(MovePlayerTowards(_nextNode));
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
            _selection.transform.position = _oSelectionPos;
            _selection.SetActive(false);
        }

        private void Update()
        {
            _rb.maxLinearVelocity = _maxSpeed;
        }

        IEnumerator MovePlayerTowards(Transform target)
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
}