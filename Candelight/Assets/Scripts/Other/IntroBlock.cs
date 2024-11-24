using DG.Tweening;
using Hechizos;
using Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class IntroBlock : MonoBehaviour
{
    [SerializeField] GameObject _block;
    [SerializeField] GameObject _runes;

    UIManager _ui;

    //TODO
    //Recuerda cambiar de nuevo el material para que no salga el highlight

    private void Awake()
    {
        _ui = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _block.SetActive(true);
            _block.transform.DOMoveY(1.25f, 0.5f).Play();

            _runes.SetActive(true);
            _runes.transform.DOMoveY(1.5f, 4f).Play();

            StartCoroutine(BookHelp());
        }
    }

    IEnumerator BookHelp()
    {
        if (ARune.FindSpell("Projectile", out var spell))
        {
            while (!spell.IsActivated())
            {
                yield return new WaitForSecondsRealtime(15f);
                if (!spell.IsActivated()) _ui.ShowTutorial("Pulsa B para abrir tus apuntes.\n\nMantén SHIFT o ESPACIO e invoca las runas para memorizar una nueva magia.");
            }

            StartCoroutine(SpellHelp());
        }
    }

    IEnumerator SpellHelp()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(15f);
            _ui.ShowTutorial("Para activar un elemento memorizado, mantén ESPACIO e invoca sus runas.\n Para lanzar un hechizo memorizado, mantén SHIFT e invoca sus runas.");
        }
    }

    public void ResetBlock()
    {
        StopAllCoroutines();
        FindObjectOfType<TutorialRoom>().SpawnEnd();

        GetComponent<SpriteRenderer>().sprite = null;
        foreach (var c in GetComponents<Collider>()) c.enabled = false;

        _block.transform.DOMoveY(-2f, 1f).Play();

        _runes.transform.DOMoveY(-2f, 1f).Play().OnComplete(End);
    }

    void End()
    {
        _runes.SetActive(false);
        _block.SetActive(false);
        Destroy(gameObject);
    }
}
