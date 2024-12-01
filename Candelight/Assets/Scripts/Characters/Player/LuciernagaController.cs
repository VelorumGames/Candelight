using Animations;
using Enemy;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuciernagaController : MonoBehaviour
{
    public float speed = 5f;                // Velocidad de movimiento de la luci�rnaga
    public float enemyDetectionRange = 10f; // Distancia para detectar al enemigo
    public float followDistance = 1.5f;     // Distancia m�nima respecto al jugador
    
    public float floatSpeed = 1f;           // Velocidad hacia arriba y abajo (efecto flotar)
    public float minHeight = 2.5f;          // Altura m�nima en el eje Y (efecto flotar)
    public float maxHeight = 3f;            // Altura m�xima en el eje Y (efecto flotar)
    private float YPosition;          // Posici�n respectoa a la altura (efecto flotar)

    private Transform player;               // Referencia al jugador
    private EnemyController enemy;          // Referencia al controlador del enemigo actual

    private Vector3 targetPosition;         // Posici�n objetivo
    private enum LuciernagaState { Revoloteando, AvanzandoAEnemigo, Posada }
    private LuciernagaState estado = LuciernagaState.Revoloteando;

    LuciernagaAnimation _anim;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        _anim = GetComponentInChildren<LuciernagaAnimation>();
    }

    //private void Start()
    //{
    //    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    //    if (playerObject != null)
    //    {
    //        player = playerObject.transform;
    //    }
    //    else
    //    {
    //        Debug.LogError("No se encontr� ning�n objeto con el tag 'Player'.");
    //    }
    //}

    private void Update()
    {
        FSMLuciernaga();
    }

    private void FSMLuciernaga()
    {
        switch (estado)
        {
            case LuciernagaState.Revoloteando:
                // Movimiento cerca del jugador
                if (player != null)
                {
                    targetPosition = player.position + (transform.position - player.position).normalized * followDistance;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                    // Efecto flotar en el eje Y
                    YPosition = Mathf.Lerp(minHeight, maxHeight, Mathf.PingPong(Time.time * floatSpeed, 1));
                    transform.position = new Vector3(transform.position.x, YPosition, transform.position.z);
                }

                // Detectar enemigos en rango
                if (DetectarEnemigos())
                {
                    estado = LuciernagaState.AvanzandoAEnemigo;
                    Debug.Log("Cambiando al estado: AvanzandoAEnemigo");
                }
                break;

            case LuciernagaState.AvanzandoAEnemigo:
                if (enemy != null)
                {
                    // Moverse hacia el enemigo
                    transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);

                    // Si alcanza al enemigo, cambia al estado Posada
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= 0.1f)
                    {
                        if (!enemy.LuciernagaPosada) // Verificar si el enemigo ya tiene una luci�rnaga posada
                        {
                            _anim.GetToEnemy(true);

                            estado = LuciernagaState.Posada;
                            enemy.LuciernagaPosada = true; // Marcar al enemigo como ocupado
                            Debug.Log("Cambiando al estado: Posada");
                        }
                        else
                        {
                            _anim.GetToEnemy(false);

                            // Si el enemigo ya tiene una luci�rnaga posada, vuelve a Revoloteando
                            estado = LuciernagaState.Revoloteando;
                            Debug.Log("Enemigo ya ocupado, cambiando a Revoloteando");
                        }
                    }
                }
                else
                {
                    // Si no hay enemigo, volver a Revoloteando
                    estado = LuciernagaState.Revoloteando;
                    Debug.Log("Enemigo perdido. Cambiando al estado: Revoloteando");
                }
                break;

            case LuciernagaState.Posada:
                if (enemy != null)
                {
                    // Mantenerse en la posici�n del enemigo
                    transform.position = enemy.transform.position;
                }
                else
                {
                    // Si el enemigo desaparece, volver al estado Revoloteando
                    estado = LuciernagaState.Revoloteando;
                    Debug.Log("Enemigo desaparecido. Cambiando al estado: Revoloteando");
                }
                break;

            default:
                Debug.LogError("Estado no reconocido en la FSM.");
                break;
        }
    }

    private bool DetectarEnemigos()
    {
        // Buscar enemigos en el rango de detecci�n usando Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyDetectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemy = hitCollider.GetComponent<EnemyController>();

                // Solo seleccionar enemigos que no tengan una luci�rnaga posada
                if (enemy != null && !enemy.LuciernagaPosada)
                {
                    _anim.SetEnemyFound(true);
                    return true; // Enemigo v�lido detectado
                }
            }
        }

        // Si no hay enemigos v�lidos en el rango
        enemy = null;
        _anim.SetEnemyFound(false);
        return false;
    }
}
