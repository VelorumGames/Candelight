using Animations;
using Enemy;
using Hechizos;
using Hechizos.Elementales;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperManIA : EnemyController
{
    [SerializeField] private float _visionDistance;

    [SerializeField] private float _playerNearDistance;
    [SerializeField] private float _canFleeDistance;
    [SerializeField] private float _playerAtRangeDistance;

    [SerializeField] private float speed;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletSpeed;

    private bool _isAngry;
    [SerializeField] private float _angryCounterTime;
    private float _angryCounter;

    CopperManAnimation _anim;

    protected new void Awake()
    {
        base.Awake();
        _isAngry = true;
        _anim = GetComponentInChildren<CopperManAnimation>();
        StartCoroutine(CheckPerceptions());
    }

    private IEnumerator CheckPerceptions()
    {
        while (true)
        {
            bool playerNear, canFlee, playerAtRange, playerCanBeSeen;
            float distToPlayer = Vector3.Distance(Player.transform.position, transform.position);
            //var hits = Physics.RaycastAll(transform.position, Player.transform.position - transform.position, _visionDistance);

            playerAtRange = distToPlayer < _playerAtRangeDistance;
            playerCanBeSeen = distToPlayer < _visionDistance;
            playerNear = distToPlayer < _playerNearDistance;
            canFlee = distToPlayer < _canFleeDistance;

            //if (Physics.OverlapSphere(transform.position, _playerNearDistance, 6) != null)
            //{
            //    playerNear = true;
            //    canFlee = Physics.Raycast(transform.position, transform.position - Player.transform.position, _canFleeDistance);
            //}
            //playerAtRange = Physics.OverlapSphere(transform.position, _playerAtRangeDistance, 6) != null;

            Debug.Log($"ESTA ENFADADO: {_isAngry}\nVE AL JUGADOR: {playerCanBeSeen}\nJUGADOR CERCA: {playerNear}");

            if (_isAngry)
            {
                if (playerCanBeSeen)
                {
                    if (playerNear)
                    {
                        if (canFlee) StartCoroutine(MoveToPosition(transform.position - Player.transform.position));
                    }
                    else
                    {
                        Debug.Log("Dispara proyectil");
                        var proyectile = Instantiate(_bullet).GetComponent<Projectile>();
                        proyectile.Push((Player.transform.position - transform.position).normalized, _bulletSpeed);
                    }
                }
            }
            else
            {
                if (playerCanBeSeen)
                {
                    if ((Player.transform.position - transform.position).magnitude < 1.5f)
                        Player.RecieveDamage(Info.BaseDamage);
                    else
                        StartCoroutine(MoveToPosition(Player.transform.position));
                }
                else
                {
                    float xMod = Random.Range(-5f, 5f);
                    float zMod = Random.Range(-5f, 5f);
                    StartCoroutine(MoveToPosition(new Vector3(transform.position.x + xMod, transform.position.y, transform.position.z + zMod)));
                }

            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator MoveToPosition(Vector3 pos)
    {
        Debug.Log("Me muevo hacia jugador");
        var movPerIter = (((pos - transform.position).normalized / 10f) * speed / 10) / 5f;
        for (int i = 0; i < 5; i++)
        {
            transform.position += movPerIter;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator AngryCountDown()
    {
        while (_angryCounter > 0)
        {
            _angryCounter -= Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        _isAngry = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ASpell ASpellScript))
        {
            foreach (AElementalRune spell in ASpellScript.Elements)
            {
                if (spell is ElectricRune)
                {
                    _isAngry = false;
                    _angryCounter = _angryCounterTime;
                    StartCoroutine(AngryCountDown());
                }
            }
        }
    }
}
