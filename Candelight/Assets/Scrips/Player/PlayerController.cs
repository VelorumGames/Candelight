using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _rb; // Variable para almacenar el Rigidbody del jugador
        [SerializeField] float _speed; // Velocidad del jugador
        [SerializeField] float _maxSpeed; // Velocidad máxima del jugador

        private bool _puedeMoverse = true; // Estado de movimiento del jugador

        private GlifoElemental _glifoElementalActivo; // Glifo elemental activo del jugador 

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>(); // Obtener el Rigidbody del jugador al inicio
        }

        private void Start()
        {
            _rb.maxLinearVelocity = _maxSpeed; // Establecer la velocidad máxima del Rigidbody
            
            if (_glifoElementalActivo != null)
            {
                _glifoElementalActivo.AplicarEfecto(); // Aplicar el efecto del glifo elemental activo
            }
            else
            {
                _glifoElementalActivo = new FuegoGlifo(); // Inicializa como "Fuego" por defecto
                _glifoElementalActivo.AplicarEfecto(); // Aplicar el efecto del glifo elemental
            }
        }

        // -- MOVIMIENTO --
        public bool GetMovimiento()
        {
            return _puedeMoverse; // Retornar el estado de movimiento
        }
        public void SetMovimiento(bool puedeMoverse)
        {
            _puedeMoverse = puedeMoverse; // Establecer el estado de movimiento
        }

        public void OnMove(Vector2 direction)
        {
            // Solo permite mover si puede moverse
            if (_puedeMoverse)
            {
                Vector3 force = Time.deltaTime * 100f * _speed * new Vector3(direction.x, 0f, direction.y);
                _rb.AddForce(force, ForceMode.Force); // Aplicar la fuerza calculada al Rigidbody
            }
        }

        // -- GLIFOS --
        public void ActivarGlifo(GlifoElemental glifo)
        {
            _glifoElementalActivo = glifo; // Establecer el glifo elemental activo
            _glifoElementalActivo.AplicarEfecto(); // Aplicar el efecto del glifo elemental
        }

        public void ActivarGlifoDeForma(GlifoDeForma glifoDeForma) // (A nivel gráfico por implementar)
        {
            glifoDeForma.AplicarEfecto();
        }

        public string ObtenerElementoActivo()
        {
            return _glifoElementalActivo.Nombre; // Retornar el nombre del glifo elemental activo
        }
    }
}
