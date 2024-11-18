using DG.Tweening;
using Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroBlock : MonoBehaviour
{
    [SerializeField] GameObject _block;
    [SerializeField] GameObject _runes;

    //TODO
    //Recuerda cambiar de nuevo el material para que no salga el highlight

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _block.SetActive(true);
            _block.transform.DOMoveY(1.25f, 0.5f).Play();

            _runes.SetActive(true);
            _runes.transform.DOMoveY(1.5f, 4f).Play();
        }
    }

    public void ResetBlock()
    {
        FindObjectOfType<TutorialRoom>().SpawnEnd();

        GetComponent<SpriteRenderer>().sprite = null;
        foreach (var c in GetComponents<Collider>()) c.enabled = false;

        _block.SetActive(true);
        _block.transform.DOMoveY(-2f, 1f).Play();

        _runes.SetActive(true);
        _runes.transform.DOMoveY(-2f, 1f).Play().OnComplete(() => Destroy(gameObject));
    }
}
