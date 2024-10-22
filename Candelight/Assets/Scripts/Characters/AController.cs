using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AController : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] protected float _speed;

    public float MaxHP;
    float m_HP;
    public float CurrentHP
    {
        get => m_HP;
        set
        {
            if (m_HP != value)
            {
                m_HP = Mathf.Clamp(value, 0, MaxHP);
            }
        }
    }

    public Vector3 Orientation;

    public abstract void RecieveDamage(float damage);

    public void OnMove(Vector2 direction)
    {
        //Debug.Log("RIGIDBODY: " + _rb);
        if (!_rb) _rb = GetComponent<Rigidbody>();
        Orientation = new Vector3(direction.x, 0f, direction.y);
        Vector3 force = Time.deltaTime * 100f * _speed * new Vector3(direction.x, 0f, direction.y);
        _rb.AddForce(force, ForceMode.Force);
    }

    protected IEnumerator MoveTowards(Transform target)
    {
        Vector3 direction = new Vector3((target.position - transform.position).x, 0f, (target.position - transform.position).z).normalized;
        while (Vector3.Distance(transform.position, target.position) > 1.5f)
        {
            OnMove(new Vector2(direction.x, direction.z));
            yield return null;
        }
    }

    protected IEnumerator MoveTowards(Vector3 target, float timeOut)
    {
        float outTime = 0;
        Vector3 direction = new Vector3((target - transform.position).x, 0f, (target - transform.position).z).normalized;
        while (Vector3.Distance(transform.position, target) > 1.5f && outTime < timeOut)
        {
            OnMove(new Vector2(direction.x, direction.z));
            outTime += Time.deltaTime;
            yield return null;
        }
    }
}
