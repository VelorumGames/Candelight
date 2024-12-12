using Animations;
using Enemy;
using Hechizos;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ECopperState
{
    Normal,
    Angry
}

public class HombreDeCobreIA : EnemyController
{
    Action _updateAction;

    ECopperState _currentState;

    float _angryTimeLimit = 8f;
    float m_timer;
    float _angryTimer
    {
        get => m_timer;
        set
        {
            m_timer = value;
            if (value >= _angryTimeLimit && _currentState == ECopperState.Normal)
            {
                ChangeToState(ECopperState.Angry);
            }
        }
    }

    float _minAngryDist = 5f;
    float _minNormalDist = 1.5f;

    [SerializeField] GameObject[] _proyectiles;
    float _projDelay = 1f;
    float _projTimer;
    float _projLifeSpan = 4f;

    CopperManAnimation _anim;

    public AudioClip[] State; //0: Angry, 1: Calma
    public AudioClip[] AttackSound; //0: Melee, 1: Shoot

    private new void Awake()
    {
        base.Awake();
        _anim = GetComponentInChildren<CopperManAnimation>();
    }

    private new void Start()
    {
        base.Start();

        ChangeToState(ECopperState.Angry);
    }

    void ChangeToState(ECopperState state)
    {
        Debug.Log("CAMBIO A ESTADO: " + state);

        _anim.EndAttacking();

        StopAllCoroutines();

        switch (state)
        {
            case ECopperState.Normal:
                Audio.PlayOneShot(State[1]);
                EnState.ShowState("CobreNormal");

                StartCoroutine(NormalBehaviour());
                _updateAction = null;
                break;
            case ECopperState.Angry:
                Audio.PlayOneShot(State[0]);
                EnState.ShowState("CobreEnfadado");

                StopAllCoroutines();
                _updateAction = AngryUpdate;
                break;
        }

        _currentState = state;
    }

    private void Update()
    {
        _angryTimer += Time.deltaTime;

        if (_updateAction != null) _updateAction();
    }

    #region Normal

    IEnumerator NormalBehaviour()
    {
        while (true)
        {
            yield return StartCoroutine(MoveTowards(Player.transform.position, 1f));
            if (Vector3.Distance(transform.position, Player.transform.position) < _minNormalDist) //Si entra en rango de ataque
            {
                yield return StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        Audio.PlayOneShot(AttackSound[0]);

        OnAttack(); //Ataca y se espera un poco para que la animacion se reproduzca con normalidad (y para darle tiempo al jugador de escapar)
        CanMove = false;
        _anim.ChangeToMelee();
        yield return new WaitForSeconds(1.5f);
        CanMove = true;
    }

    #endregion

    #region Angry

    void AngryUpdate()
    {
        Vector3 dir = transform.position - Player.transform.position;

        if (Vector3.Distance(transform.position, Player.transform.position) < _minAngryDist)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < _minNormalDist)
            {
                if (CanMove) StartCoroutine(Attack());
            }
            else
            {
                _rb.AddForce(_speed * Time.deltaTime * 120f * dir.normalized, ForceMode.Force);

                _anim.EndAttacking();
            }
        }
        else if (_projTimer >= _projDelay)
        {
            _projTimer = 0f;
            SpawnProjectile();

            _anim.ChangeToShoot();
        }

        _projTimer += Time.deltaTime;
    }

    void SpawnProjectile()
    {
        int id = UnityEngine.Random.Range(0, _proyectiles.Length);
        int timeout = 0;

        while (_proyectiles[id].activeInHierarchy && timeout++ < 50)
        {
            id = UnityEngine.Random.Range(0, _proyectiles.Length);
        }

        _proyectiles[id].transform.position = transform.position;

        Vector3 dir = transform.position - Player.transform.position;
        _proyectiles[id].GetComponent<Rigidbody>().velocity = 3.5f * -dir.normalized;
        _proyectiles[id].GetComponent<Projectile>().Damage = Info.BaseDamage * 0.3f;

        _proyectiles[id].SetActive(true);
        StartCoroutine(ResetProjectile(_proyectiles[id]));

        Audio.PlayOneShot(AttackSound[1]);
    }

    IEnumerator ResetProjectile(GameObject proj)
    {
        yield return new WaitForSeconds(_projLifeSpan);
        proj.SetActive(false);
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ASpell>(out var spell) && !spell.AffectsPlayer)
        {
            foreach (var el in spell.Elements)
            {
                if (el is ElectricRune)
                {
                    _angryTimer = 0f;
                    if (_currentState == ECopperState.Angry) ChangeToState(ECopperState.Normal);
                }
            }
        }
    }
}
