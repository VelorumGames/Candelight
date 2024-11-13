using Enemy;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : ARoom
{
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

    protected new void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
        _enemyCount = enemies.Length;
        foreach (var e in enemies)
        {
            e.OnDeath += NotifyEnemyDeath;
        }
    }

    protected override void OnPlayerTrigger()
    {
        FindObjectOfType<MapManager>().StartCombat();
        Invoke("CloseAllAnchors", 1f);
    }

    void EndCombat()
    {
        FindObjectOfType<MapManager>().EndCombat();
        OpenAllAnchors();
    }

    public void CloseAllAnchors()
    {
        foreach (var anchor in AvailableAnchors)
        {
            anchor.CloseAnchor();
        }
    }

    void OpenAllAnchors()
    {
        foreach (var anchor in AvailableAnchors)
        {
            anchor.OpenAnchor();
        }
    }

    void NotifyEnemyDeath(AController enemy)
    {
        enemy.OnDeath -= NotifyEnemyDeath;
        _enemyCount--;
    }
}
