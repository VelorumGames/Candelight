using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using World;
using Map;
using System.Drawing;
using Animations;

public class NPCController : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] protected float _speed;
    public ESpriteOrientation InitialOrientation;

    PlayerController _player;

    public bool CanMoveOnStart;
    AldeanoAnimation _anim;

    protected Vector3 Orientation;
    /// <summary>
    /// Se desplaza a un personaje en una direccion
    /// </summary>
    /// <param name="direction"></param>
    /// 

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<AldeanoAnimation>();
        _player = FindObjectOfType<PlayerController>();

        Orientation = transform.forward;
    }

    private void Start()
    {
        if (!CanMoveOnStart)
        {
            switch (InitialOrientation)
            {
                case ESpriteOrientation.Up:
                    _anim.ChangeToUp();
                    break;
                case ESpriteOrientation.Down:
                    _anim.ChangeToDown();
                    break;
                case ESpriteOrientation.Left:
                    _anim.ChangeToLeft();
                    break;
                case ESpriteOrientation.Right:
                    _anim.ChangeToRight();
                    break;
            }
        }
    }

    public float getSpeed() { return _speed; }

    public IEnumerator Move(Vector3 target)
    {
        if (CanMoveOnStart)
        {
            int count;
            while (true)
            {
                Vector3 randomPos = new Vector3(Random.Range(-3.0f, 3.0f), transform.position.y, Random.Range(-3.0f, 3.0f));

                count = 10;
                while(Mathf.Abs(randomPos.x) > 1f && Mathf.Abs(randomPos.z) > 1f && count > 0)
                {
                    randomPos = new Vector3(Random.Range(-3.0f, 3.0f), transform.position.y, Random.Range(-3.0f, 3.0f));
                    count--;
                }

                //Debug.Log("Me estoy moviendo");
                yield return StartCoroutine(MoveTowardsWithCollision(randomPos, 4.0f));
                yield return new WaitForSeconds(Random.Range(1f, 5f));
            }
        }
    }

    public IEnumerator Surprise()
    {
        StopAllCoroutines();
        yield return StartCoroutine(MoveTowards(_player.transform.position, 0.5f));
        _anim.ChangeToSurprise();
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
    protected IEnumerator MoveTowardsWithCollision(Vector3 target, float timeOut)
    {
        //Debug.Log($"Estoy en posicion {transform.position} y quiero ir a {target}");

        float outTime = 0;
        Vector3 direction;
        while (Vector3.Distance(transform.position, target) > 1.5f && outTime < timeOut)
        {
            //Debug.Log($"Estoy en posicion {transform.position} y quiero ir a {target}");
            direction = new Vector3((target - transform.position).x, 0f, (target - transform.position).z).normalized;
            OnMove(new Vector2(direction.x, direction.z));
            outTime += Time.deltaTime;

            //Debug.Log(_rb.velocity.magnitude);
            if (_rb.velocity.magnitude < 0.01f) break; //Se ha chocado

            yield return null;
        }
    }

    public IEnumerator MoveTowards(Vector3 target, float timeOut)
    {
        //Debug.Log($"Estoy en posicion {transform.position} y quiero ir a {target}");

        float outTime = 0;
        Vector3 direction;
        while (Vector3.Distance(transform.position, target) > 1.5f && outTime < timeOut)
        {
            //Debug.Log($"Estoy en posicion {transform.position} y quiero ir a {target}");
            direction = new Vector3((target - transform.position).x, 0f, (target - transform.position).z).normalized;
            OnMove(new Vector2(direction.x, direction.z));
            outTime += Time.deltaTime;

            //Debug.Log(_rb.velocity.magnitude);
            //if (_rb.velocity.magnitude < 0.01f) break; //Se ha chocado

            yield return null;
        }
    }

    public Vector3 GetOrientation()
    {
        //Debug.Log("ORIENTATION: " + Orientation);
        return Orientation.normalized;
    }

    public IEnumerator ExitRoom()
    {
        _anim.GetComponent<Collider>().enabled = false;
        GetComponentInChildren<AldeanoAnimation>().Active = true;

        //Primero se dirige hacia la salida mas lejana para apartarse del jugador
        yield return MoveTowards(FindFarestRoomFromPlayer(), 10f);

        //Despues, va directamente en la direccion opuesta al jugador. Se intuye que el jugador no vera el limite de la habitacion en la mayoria de casos

        StartCoroutine(MoveTowards(GetFarPointFromPlayer(), 10f));
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    Vector3 FindFarestRoomFromPlayer()
    {
        AnchorManager obj = null;
        float maxDist = 0f;
        float dist = 0f;

        AnchorManager[] anchors = transform.parent.GetComponentsInChildren<AnchorManager>();
        if (anchors.Length == 0) //Caso de los eventos
        {
            //           npc -> evento -> SP -> container -> Room
            anchors = transform.parent.parent.parent.parent.GetComponentsInChildren<AnchorManager>();
        }

        Debug.Log($"Se busca entre {anchors.Length} anchors");

        foreach (var anchor in anchors)
        {
            dist = Vector3.Distance(_player.transform.position, anchor.transform.position);
            if (dist > maxDist)
            {
                maxDist = dist;
                obj = anchor;
            }
        }

        Debug.Log($"Se ira al anchor: {obj.GetDirection()}");

        return obj.transform.position;
    }

    Vector3 GetFarPointFromPlayer() => (transform.position - _player.transform.position) * 10f;
}
