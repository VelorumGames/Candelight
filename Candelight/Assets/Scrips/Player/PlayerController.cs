using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _rb;
        [SerializeField] float _speed;
        [SerializeField] float _maxSpeed;

        Action _interaction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rb.maxLinearVelocity = _maxSpeed;
        }

        public void OnMove(Vector2 direction)
        {
            Vector3 force = Time.deltaTime * 100f * _speed * new Vector3(direction.x, 0f, direction.y);
            _rb.AddForce(force, ForceMode.Force);
        }

        public void OnInteract(InputAction.CallbackContext _)
        {
            _interaction();
        }

        public void LoadInteraction(Action interaction) => _interaction = interaction;

        private void Update()
        {
            _rb.maxLinearVelocity = _maxSpeed;
        }
    }
}