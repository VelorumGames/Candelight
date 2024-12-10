using Comportamientos.Sombra;
using Enemy;
using Map;
using Music;
using Player;
using SpellInteractuable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : ARoom
{
    EnemyController[] _enemies;
    SombraComportamiento[] _shadows;

    [SerializeField] int _eCount;
    int _enemyCount
    {
        get => _eCount;
        set
        {
            if (_eCount != value)
            {
                _eCount = value;
                if (value == 0) EndCombat();
            }
        }
    }

    [SerializeField] GameObject _spawnEffect;
    GameObject[] _spawns;

    bool _inCombat;

    private new void Awake()
    {
        base.Awake();

        _enemies = GetComponentsInChildren<EnemyController>(true);
        _shadows = GetComponentsInChildren<SombraComportamiento>(true);

        _enemyCount = _enemies.Length + _shadows.Length;
        foreach (var e in _enemies)
        {
            e.OnDeath += NotifyEnemyDeath;
            e.gameObject.SetActive(false);
        }
        foreach (var s in _shadows)
        {
            s.OnDeath += NotifyShadowDeath;
        }

        _spawns = new GameObject[_enemies.Length];
    }

    private void OnEnable()
    {
        OnPlayerExit += ResetCheck;
    }

    protected override void OnPlayerTrigger()
    {
        if (!_inCombat)
        {
            if (_enemyCount > 0)
            {
                StartCoroutine(CheckForPlayerDistance());
            }
        }
    }

    IEnumerator CheckForPlayerDistance()
    {
        yield return new WaitUntil(() => Vector3.Distance(_cont.transform.position, transform.position) < 3f);

        FindObjectOfType<MapManager>().StartCombat();
        CloseAllAnchors();

        int count = 0;
        foreach (var e in _enemies)
        {
            _spawns[count++] = Instantiate(_spawnEffect, e.transform.position - Vector3.up, Quaternion.Euler(-90f, 0f, 0f));
        }

        yield return new WaitForSeconds(1f);

        foreach (var s in _spawns) Destroy(s);

        foreach (var e in _enemies)
        {
            e.gameObject.SetActive(true);
        }
        foreach (var s in _shadows)
        {
            s.gameObject.SetActive(true);
        }

        _inCombat = true;
    }

    void ResetCheck() => StopAllCoroutines();

    void EndCombat()
    {
        FindObjectOfType<MapManager>().EndCombat();
        OpenAllAnchors();
    }

    public void CloseAllAnchors()
    {
        foreach (var anchor in AvailableAnchors)
        {
            if (anchor != null)
            {
                anchor.CloseAnchor();
                anchor.GetComponent<SI_AnchorBarrier>().Activate(true);
            }
        }
    }

    void OpenAllAnchors()
    {
        //Debug.Log($"Se abriran {AvailableAnchors.Count} anchors");
        foreach (var anchor in AvailableAnchors)
        {
            //Debug.Log("Anchor: " + anchor);
            if (anchor != null)
            {
                anchor.OpenAnchor();
                anchor.GetComponent<SI_AnchorBarrier>().Activate(false);
                anchor.GetComponentInChildren<HealthBar>()?.gameObject.SetActive(false);
            }
        }
    }

    void NotifyEnemyDeath(AController enemy)
    {
        enemy.OnDeath -= NotifyEnemyDeath;
        _enemyCount--;
    }

    void NotifyShadowDeath(SombraComportamiento shadow)
    {
        shadow.OnDeath -= NotifyShadowDeath;
        _enemyCount--;
    }

    private void OnDisable()
    {
        foreach (var e in _enemies)
        {
            if (e != null) e.OnDeath -= NotifyEnemyDeath;
        }
        foreach (var s in _shadows)
        {
            if (s != null) s.OnDeath -= NotifyShadowDeath;
        }

        OnPlayerExit -= ResetCheck;
    }
}
