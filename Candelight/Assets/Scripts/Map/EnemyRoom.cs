using Enemy;
using Map;
using Music;
using SpellInteractuable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : ARoom
{
    EnemyController[] enemies;

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

    bool _inCombat;

    private void OnEnable()
    {
        enemies = GetComponentsInChildren<EnemyController>();
        _enemyCount = enemies.Length;
        foreach (var e in enemies)
        {
            e.OnDeath += NotifyEnemyDeath;
            e.gameObject.SetActive(false); 
        }

        OnPlayerExit += ResetCheck;
    }

    protected override void OnPlayerTrigger()
    {
        if (!_inCombat)
        {
            if (_enemyCount > 0)
            {
                FindObjectOfType<MapManager>().StartCombat();
                StartCoroutine(CheckForPlayerDistance());
            }

            _inCombat = true;
        }
    }

    IEnumerator CheckForPlayerDistance()
    {
        yield return new WaitUntil(() => Vector3.Distance(_cont.transform.position, transform.position) < 2f);
        CloseAllAnchors();
        foreach (var e in enemies)
        {
            e.gameObject.SetActive(true);
        }
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
        Debug.Log($"Se abriran {AvailableAnchors.Count} anchors");
        foreach (var anchor in AvailableAnchors)
        {
            Debug.Log("Anchor: " + anchor);
            if (anchor != null)
            {
                anchor.OpenAnchor();
                anchor.GetComponent<SI_AnchorBarrier>().Activate(false);
            }
        }
    }

    void NotifyEnemyDeath(AController enemy)
    {
        enemy.OnDeath -= NotifyEnemyDeath;
        _enemyCount--;
    }

    private void OnDisable()
    {
        foreach (var e in enemies)
        {
            if (e != null) e.OnDeath -= NotifyEnemyDeath;
        }

        OnPlayerExit -= ResetCheck;
    }
}
