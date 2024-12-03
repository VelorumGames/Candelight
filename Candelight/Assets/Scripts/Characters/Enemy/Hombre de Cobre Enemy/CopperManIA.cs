using Enemy;
using Hechizos;
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

    protected void Awake()
    {
        base.Awake();
        _isAngry = false;
        StartCoroutine(CheckPerceptions());
    }

    private IEnumerator CheckPerceptions()
    {
        while (true)
        {
            bool playerNear = false, canFlee = false, playerAtRange = false, playerCanBeSeen = false;
            var hits = Physics.RaycastAll(transform.position, Player.transform.position - transform.position, _visionDistance);
            if (hits.Length == 1 && hits[0].collider.gameObject.GetComponent<PlayerController>() != null)
                playerCanBeSeen = true;

            if (Physics.OverlapSphere(transform.position, _playerNearDistance, 6) != null)
            {
                playerNear = true;
                canFlee = Physics.Raycast(transform.position, transform.position - Player.transform.position, _canFleeDistance);
            }
            playerAtRange = Physics.OverlapSphere(transform.position, _playerAtRangeDistance, 6) != null;

            if (_isAngry)
            {
                if (playerCanBeSeen)
                {
                    if (playerNear)
                    {
                        if (canFlee)
                            StartCoroutine(MoveToPosition(transform.position - Player.transform.position));
                    }
                    else
                    {
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
                        Player.GetComponent<PlayerController>().RecieveDamage(Info.BaseDamage);
                    else
                        StartCoroutine(MoveToPosition(Player.transform.position));
                }
                else
                {
                    float xMod = Random.Range(-5, 5);
                    float zMod = Random.Range(-5, 5);
                    StartCoroutine(MoveToPosition(new Vector3(transform.position.x + xMod, transform.position.y, transform.position.z + zMod)));
                }

            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator MoveToPosition(Vector3 pos)
    {

        var movPerIter = (((pos - transform.position).normalized / 10f) * speed / 10) / 5f;
        for (int i = 0; i < 5; i++)
        {
            transform.position += movPerIter;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
