using DG.Tweening;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUntilRune : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitUntil(CheckForProjRune);

        Vector3 newPos = transform.localPosition - 5f * Vector3.up;
        transform.DOLocalMove(newPos, 5f).OnComplete(() => Destroy(gameObject));
    }

    bool CheckForProjRune() => ARune.FindSpell("Projectile", out var spell) && spell.IsActivated();
}
