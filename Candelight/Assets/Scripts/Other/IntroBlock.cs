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

    bool _active = true;

    //TODO
    //Recuerda cambiar de nuevo el material para que no salga el highlight

    private void Awake()
    {
        _ui = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _active)
        {
            _active = false;

            _block.SetActive(true);
            _block.transform.DOMoveY(1.25f, 0.5f).Play();

            _runes?.SetActive(true);
            _runes?.transform.DOMoveY(1.5f, 4f).Play();

            StartCoroutine(BookHelp());
        }
    }

    IEnumerator BookHelp()
    {
        if (ARune.FindSpell("Projectile", out var spell))
        {
            while (!spell.IsActivated())
            {
                if (!spell.IsActivated())
                {
                    if (GameSettings.HelpMessages)
                    {
                        if (Application.isMobilePlatform) _ui.ShowTutorial("Pulsa el icono del libro para tus apuntes.\nMantén pulsado uno de los símbolos (abajo a la izquierda) e invoca las runas para memorizar una nueva magia.", 10f);
                        else _ui.ShowTutorial("Pulsa B para abrir tus apuntes.\nMantén CLICK e invoca las runas para memorizar una nueva magia.", 10f);
                    }
                    yield return new WaitForSecondsRealtime(13f);
                }
            }

            StartCoroutine(SpellHelp());
        }
    }

    IEnumerator SpellHelp()
    {
        while (true)
        {
            if (GameSettings.HelpMessages)
            {
                if (Application.isMobilePlatform) _ui.ShowTutorial("Cierra tus apuntes pulsando B de nuevo.\nPara lanzar un hechizo, mantén el símbolo de forma e invoca sus runas.", 10f);
                else _ui.ShowTutorial("Cierra tus apuntes pulsando el libro de nuevo.\nPara lanzar un hechizo, mantén CLK IZQ e invoca sus runas.", 10f);
            }
            yield return new WaitForSecondsRealtime(13f);
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
