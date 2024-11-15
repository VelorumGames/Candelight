using Enemy;
using Map;
using Music;
using SpellInteractuable;
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
        if (_enemyCount > 0)
        {
            FindObjectOfType<MapManager>().StartCombat();
            Invoke("CloseAllAnchors", 1f);
        }
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
            anchor.GetComponent<SI_AnchorBarrier>().Activate(true);
        }
    }

    void OpenAllAnchors()
    {
        Debug.Log($"Se abriran {AvailableAnchors.Count} anchors");
        foreach (var anchor in AvailableAnchors)
        {
            anchor.OpenAnchor();
            anchor.GetComponent<SI_AnchorBarrier>().Activate(false);
        }
    }

    void NotifyEnemyDeath(AController enemy)
    {
        enemy.OnDeath -= NotifyEnemyDeath;
        _enemyCount--;
    }
}
