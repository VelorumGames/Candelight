using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public abstract class AController : MonoBehaviour
{
    public WorldInfo World;

    protected Rigidbody _rb;
    [SerializeField] protected float _speed;

    public float MaxHP;
    [SerializeField] float m_HP;
    public float CurrentHP
    {
        get => m_HP;
        set
        {
            if (m_HP != value)
            {
                m_HP = Mathf.Clamp(value, 0, MaxHP);
                if (m_HP == 0)
                {
                    if (OnDeath != null) OnDeath(this);
                    Destroy(gameObject);
                }
            }
        }
    }

    protected Vector3 Orientation;

    Coroutine _tempDamage;
    Coroutine _paralize;
    Coroutine _burn;
    Coroutine _slowness;

    protected bool CanMove = true;

    public event Action<float, float> OnDamage; //Primer float: el dano; Segundo float: la vida restante
    public event Action<AController> OnDeath;

    protected void Start()
    {
        CurrentHP = MaxHP;
    }

    #region Damage & Effects

    protected void CallDamageEvent(float dam, float health)
    {
        if (OnDamage != null) OnDamage(dam, health);
    }

    public abstract void RecieveDamage(float damage);

    public void RecieveTemporalDamage(float damage, float time, float interval)
    {
        if (_tempDamage != null) StopCoroutine(_tempDamage);
        _tempDamage = StartCoroutine(ProcessTempDamage(damage, time, interval));
    }

    IEnumerator ProcessTempDamage(float dam, float time, float interval)
    {
        while (time > 0)
        {
            RecieveDamage(dam);
            yield return new WaitForSeconds(interval);
            time -= interval;
        }
    }

    public void Paralize(float time)
    {
        if (_paralize != null) StopCoroutine(_paralize);
        _paralize = StartCoroutine(ProcessParalize(time));
    }

    IEnumerator ProcessParalize(float time)
    {
        CanMove = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(time);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        CanMove = true;
    }
    public void Burn(float time)
    {
        if (_burn != null) StopCoroutine(_burn);
        _burn = StartCoroutine(ProcessBurn(time));
    }
    IEnumerator ProcessBurn(float time)
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(time);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }


    public void Slow(float ratio, float time)
    {
        if (_slowness != null) StopCoroutine(_slowness);
        _slowness = StartCoroutine(ProcessSlowness(ratio, time));
    }

    IEnumerator ProcessSlowness(float ratio, float time)
    {
        float oSpeed = _speed;
        _speed *= ratio;
        yield return new WaitForSeconds(time);
        _speed = oSpeed;
    }

    public void Push(float force, Vector3 direction)
    {
        _rb.AddForce(force * direction.normalized, ForceMode.Impulse);
    }

    #endregion

    /// <summary>
    /// Se desplaza a un personaje en una direccion
    /// </summary>
    /// <param name="direction"></param>
    public void OnMove(Vector2 direction)
    {
        if (CanMove)
        {
            if (!_rb) _rb = GetComponent<Rigidbody>();
            Vector3 force = Time.deltaTime * 100f * _speed * new Vector3(direction.x, 0f, direction.y);
            _rb.AddForce(force, ForceMode.Force);
        }
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

    public void SetMove(bool b) => CanMove = b;

    private void OnDisable()
    {
        OnDeath = null;
    }
}
