using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using World;

public class NPCController : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] protected float _speed;

    PlayerController _player;

    protected Vector3 Orientation;
    /// <summary>
    /// Se desplaza a un personaje en una direccion
    /// </summary>
    /// <param name="direction"></param>
    /// 

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Orientation = transform.forward;
    }

    private new void Start()
    {
        _player = FindObjectOfType<PlayerController>();

        //StartCoroutine(DebugAI());
    }

    public IEnumerator Move(Vector3 target)
    {
        while (true)
        {
            Debug.Log("Me estoy moviendo");
            yield return StartCoroutine(MoveTowards(target, 10.0f));
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnMove(Vector2 direction)
    {
        if (!_rb) _rb = GetComponent<Rigidbody>();
            Vector3 force = Time.deltaTime * 100f * _speed * new Vector3(direction.x, 0f, direction.y);
            _rb.AddForce(force, ForceMode.Force);
    }

    /// <summary>
    /// Se desplaza a un personaje hacia un objetivo
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>

    protected IEnumerator MoveTowards(Transform target)
    {
        Vector3 direction;
        while (Vector3.Distance(transform.position, target.position) > 1.5f)
        {
            direction = new Vector3((target.position - transform.position).x, 0f, (target.position - transform.position).z).normalized;
            OnMove(new Vector2(direction.x, direction.z));
            yield return null;
        }
    }

    /// <summary>
    /// Se desplaza al personaje hacia un objetivo pero deja de intentar aproximarse pasado un tiempo
    /// </summary>
    /// <param name="target"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    protected IEnumerator MoveTowards(Vector3 target, float timeOut)
    {
        float outTime = 0;
        Vector3 direction;
        while (Vector3.Distance(transform.position, target) > 1.5f && outTime < timeOut)
        {
            direction = new Vector3((target - transform.position).x, 0f, (target - transform.position).z).normalized;
            OnMove(new Vector2(direction.x, direction.z));
            outTime += Time.deltaTime;
            yield return null;
        }
    }

    public Vector3 GetOrientation()
    {
        //Debug.Log("ORIENTATION: " + Orientation);
        return Orientation.normalized;
    }

    IEnumerator DebugAI()
    {
        //Debug.Log("Se inicia IA de prueba");
        while (true)
        {
            Vector3 target = transform.position + new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
            //Debug.Log("Se encuentra nuevo target: " + target);
            if (Random.value < 0.5f) yield return StartCoroutine(MoveTowards(target, 5f));
            else
            {
                if (Vector3.Distance(transform.position, _player.transform.position) < 5f)
                {
                    //Debug.Log("Enemigo va a atacar");
                    yield return StartCoroutine(MoveTowards(_player.transform));
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }
}
