using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;

namespace Controls
{
    public enum EControlMap
    {
        Level
    }

    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public InputActionAsset Input;

        [SerializeField] EControlMap _initialControls;
        PlayerController _cont;

        InputActionMap _currentMap;
        InputActionMap _levelMap;

        InputAction _move;

        // -- MODO GLIFOS --
        InputAction _modoGlifos;

        // Lista de direcciones posibles para realizar glifos
        private readonly string[] _direcciones = { "UP", "DOWN", "LEFT", "RIGHT" };

        // Diccionario que almacenará las combinaciones de teclas para cada glifo
        private Dictionary<string, List<string>> _combinacionesGlifos;

        // Lista para almacenar las teclas presionadas por el jugador
        private List<string> _combinacionActual = new List<string>();

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _cont = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            InitializeControls();
            LoadControls(_initialControls);

            // Inicializar semilla aleatoria para asegurar combinaciones diferentes
            Random.InitState(System.DateTime.Now.Millisecond);

            // Generar combinaciones aleatorias para los glifos
            GenerarCombinacionesAleatorias();
        }

        void InitializeControls()
        {
            _levelMap = Input.FindActionMap("Level");

            _move = _levelMap.FindAction("Move");
            _modoGlifos = _levelMap.FindAction("ModoGlifos");
        }

        public void LoadControls(EControlMap map)
        {
            if (_currentMap != null) _currentMap.Disable();
            switch (map)
            {
                case EControlMap.Level:
                    _currentMap = _levelMap;
                    break;
            }
            _currentMap.Enable();
        }

        private void Update()
        {
            // Mientras Ctrl esté presionado, capturamos las entradas del jugador
            if (_modoGlifos.IsPressed())
            {
                _cont.SetMovimiento(false); // Desactivar movimiento

                // Capturar entradas de teclas mientras Ctrl está presionado
                CheckArrowKeys();
            }
            else if (_modoGlifos.WasReleasedThisFrame())
            {
                // Al soltar Ctrl, se verifica la combinación actual
                VerificarCombinacion();
                _combinacionActual.Clear(); // Limpiar la combinación realizada por el jugador después de verificarla
                _cont.SetMovimiento(true); // Reactivar movimiento del jugador
            }

            // Movimiento del jugador (fuera del modo de glifos)
            if (!_modoGlifos.IsPressed() && _move.IsPressed())
            {
                _cont.OnMove(_move.ReadValue<Vector2>());
            }
        }

        private void CheckArrowKeys()
        {
            // Verificar si se presionan las teclas de flecha
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                AgregarTecla("UP");
            }
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                AgregarTecla("DOWN");
            }
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                AgregarTecla("LEFT");
            }
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                AgregarTecla("RIGHT");
            }
        }

        private void AgregarTecla(string tecla)
        {
            // Agregar la tecla presionada a la combinación actual
            _combinacionActual.Add(tecla);
        }

        // Generar combinaciones aleatorias para cada glifo al inicio de la partida
        private void GenerarCombinacionesAleatorias()
        {
            _combinacionesGlifos = new Dictionary<string, List<string>>();

            // Generar combinación para el primer glifo y agregarlo al diccionario
            _combinacionesGlifos["Fuego"] = GenerarCombinacionRandom();

            // Generar combinaciones para los demás glifos asegurándose de que sean únicas
            foreach (string glifo in new[] { "Fantasmal", "Electricidad", "Cosmico", "Proyectil", "CuerpoACuerpo", "Explosion", "Potenciacion" })
            {
                List<string> nuevaCombinacion;

                // Asegurarse de que la nueva combinación no se repita
                do
                {
                    nuevaCombinacion = GenerarCombinacionRandom();
                } while (CombinacionYaExiste(nuevaCombinacion));

                _combinacionesGlifos[glifo] = nuevaCombinacion;
            }

            // -- MOSTRAR POR PANTALLA LAS COMBINACIONES GENERADAS AL AZAR --
            foreach (var glifo in _combinacionesGlifos)
            {
                Debug.Log($"Glifo: {glifo.Key}, Combinación: {string.Join(", ", glifo.Value)}");
            }
        }

        // Función para generar una combinación aleatoria de entre 1 y 3 teclas
        private List<string> GenerarCombinacionRandom()
        {
            List<string> combinacion = new List<string>();
            int numTeclas = Random.Range(1, 4); // Generar combinaciones de entre 1 y 3 teclas

            for (int i = 0; i < numTeclas; i++)
            {
                // Elegir aleatoriamente una dirección
                string direccion = _direcciones[Random.Range(0, _direcciones.Length)];
                combinacion.Add(direccion);
            }
            return combinacion;
        }

        private bool CombinacionYaExiste(List<string> combinacion)
        {
            // Comprobar si la combinación ya está en el diccionario
            foreach (var glifo in _combinacionesGlifos.Values)
            {
                if (EsCombinacionIgual(glifo, combinacion))
                {
                    return true;
                }
            }
            return false;
        }

        private bool EsCombinacionIgual(List<string> combinacion1, List<string> combinacion2)
        {
            if (combinacion1.Count != combinacion2.Count) return false;

            for (int i = 0; i < combinacion1.Count; i++)
            {
                if (combinacion1[i] != combinacion2[i]) return false;
            }

            return true;
        }

        private void VerificarCombinacion()
        {
            // Verificar si la combinación actual coincide con alguna de las combinaciones generadas
            foreach (var glifo in _combinacionesGlifos)
            {
                if (EsCombinacionValida(glifo.Value))
                {
                    // Ejecutar el glifo correspondiente si la combinación es válida
                    EjecutarGlifo(glifo.Key);
                    return; // Si se encuentra una combinación válida, detener la búsqueda
                }
            }
        }

        private bool EsCombinacionValida(List<string> combinacion)
        {
            if (_combinacionActual.Count != combinacion.Count) return false;

            // Verificar si las teclas presionadas coinciden con la combinación almacenada
            for (int i = 0; i < combinacion.Count; i++)
            {
                if (_combinacionActual[i] != combinacion[i]) return false;
            }

            return true;
        }

        private void EjecutarGlifo(string tipoGlifo)
        {
            switch (tipoGlifo)
            {
                case "Fuego":
                    _cont.ActivarGlifo(new FuegoGlifo());
                    break;
                case "Fantasmal":
                    _cont.ActivarGlifo(new FantasmalGlifo());
                    break;
                case "Electricidad":
                    _cont.ActivarGlifo(new ElectricidadGlifo());
                    break;
                case "Cosmico":
                    _cont.ActivarGlifo(new CosmicoGlifo());
                    break;

                case "Proyectil":
                    _cont.ActivarGlifoDeForma(new ProyectilGlifo(_cont.ObtenerElementoActivo()));
                    break;
                case "CuerpoACuerpo":
                    _cont.ActivarGlifoDeForma(new CuerpoACuerpoGlifo(_cont.ObtenerElementoActivo()));
                    break;
                case "Explosion":
                    _cont.ActivarGlifoDeForma(new ExplosionGlifo(_cont.ObtenerElementoActivo()));
                    break;
                case "Potenciacion":
                    _cont.ActivarGlifoDeForma(new PotenciacionGlifo(_cont.ObtenerElementoActivo()));
                    break;
            }

        }
    }
}

