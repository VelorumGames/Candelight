using Enemy;
using DG.Tweening;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyTrigger : MonoBehaviour
{
    [SerializeField] GameObject _runes;
    public EnemyController Enemy;
    public GameObject EntryBlock;

    private void OnEnable()
    {
        Enemy.OnDeath += DestroySelf;
    }

    void DestroySelf(AController _)
    {
        _runes.SetActive(true);
        _runes.transform.DOMoveY(1.5f, 4f).Play();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.gameObject.SetActive(true);
            EntryBlock.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Enemy.OnDeath -= DestroySelf;
    }
}
