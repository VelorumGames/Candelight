using Controls;
using DG.Tweening;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroTutorial : MonoBehaviour
{
    UIManager _ui;

    public InputActionAsset _controls;

    public Image EnergyImage;
    public GameObject Background;
    public Image[] FireRunes;
    public Image Element;

    public Sprite[] SymbolSprites;

    public TextMeshProUGUI DramaticText;
    public TextMeshProUGUI InstrText;
    public string _playerInstructions;
    public string _runeInstructions;

    float _spellTime;
    float _maxSpellTime = 2f;
    bool _spellModeTest;

    bool _instrTest;

    bool _inTutorial = true;

    private void Awake()
    {
        _ui = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        _ui.FadeFromBlack(1.5f, 3f);

        DramaticText.color = new Color(1f, 1f, 1f, 0f);

        InstrText.text = "";
        DramaticText.text = "";

        //Debug.Log("ENERGY IMAGE: " + EnergyImage.gameObject.name);

        //Debug. Deberia estar desactivado
        //_ui.FadeToWhite(1f, () => StartCoroutine(Tutorial()));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_inTutorial)
        {
            if (other.CompareTag("Player"))
            {
                _ui.FadeToWhite(7f, () => StartCoroutine(Tutorial()));
            }

            _inTutorial = false;
        }
    }

    IEnumerator Tutorial()
    {
        Background.SetActive(true);
        //EnergyImage.gameObject.SetActive(true);
        EnergyImage.DOFade(1f, 0.1f).Play();

        yield return new WaitForSeconds(1f);

        _ui.FadeFromWhite(1.5f);

        yield return new WaitForSeconds(3f);

        ShowText("[ B ]");
        yield return new WaitUntil(() => Keyboard.current.bKey.isPressed);
        HideText();

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(SpellTest());

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(SpellTest2());

        yield return new WaitForSeconds(1f);

        Element.sprite = SymbolSprites[0];

        ShowText("\\/[ ESPACIO ]\\/\n[ W, A, S, D ]");
        GetRuneString();
        ActivateInstructionsText();
        yield return StartCoroutine(ShowSpell());
        EndInstructions();
        HideText();

        //_ui.FadeToBlack(1f, () =>
        //{
        //    EnergyImage.gameObject.SetActive(false);
        //    _ui.FadeFromBlack(1f);
        //});
        EnergyImage.DOFade(0f, 2f);

        Debug.Log("ANIMACION DE SER UNO CON LA LLAMA");

        yield return new WaitForSeconds(3f);

        _ui.FadeToWhite(2f, () =>
        {
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
            FindObjectOfType<IntroGravity>().ResetPlayer();
            SceneManager.LoadScene("TutorialScene");
        });
    }

    void ShowText(string t)
    {
        DramaticText.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        DramaticText.text = t;
        DramaticText.DOFade(1f, 0.5f).Play();
    }

    void HideText()
    {
        DramaticText.GetComponent<RectTransform>().DOScale(1.5f, 0.5f).SetEase(Ease.OutCirc).Play();
        DramaticText.DOFade(0f, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    void ManageEnterSpell()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            _spellTime += Time.deltaTime;
            Element.color = new Color(1f, 1f, 1f, 0.5f * _spellTime / _maxSpellTime);
        }
        else
        {
            _spellTime = 0f;
            if (Element.color.a > 0) Element.DOFade(0f, 0.1f);
        }

        if (_spellTime >= _maxSpellTime) _spellModeTest = true;
    }

    void ManageEnterSpell2()
    {
        if (Keyboard.current.shiftKey.isPressed)
        {
            _spellTime += Time.deltaTime;
            Element.color = new Color(1f, 1f, 1f, 0.5f * _spellTime / _maxSpellTime);
        }
        else
        {
            _spellTime = 0f;
            if (Element.color.a > 0) Element.DOFade(0f, 0.1f);
        }

        if (_spellTime >= _maxSpellTime) _spellModeTest = true;
    }

    IEnumerator ShowSpell()
    {
        while(!_instrTest)
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                _spellTime += Time.deltaTime;
                Element.color = new Color(1f, 1f, 1f, 0.5f * _spellTime / _maxSpellTime);
                foreach (var rune in FireRunes) rune.color = new Color(1f, 1f, 1f, _spellTime / _maxSpellTime);
            }
            else
            {
                _spellTime = 0f;
                if (Element.color.a > 0) Element.DOFade(0f, 0.1f);

                if (_playerInstructions == _runeInstructions)
                {
                    _instrTest = true;
                }
                else
                {
                    foreach (var rune in FireRunes)
                    {
                        rune.DOFade(0f, 1f).Play();
                    }

                    _playerInstructions = "";
                    InstrText.text = "";
                }
            }
            yield return null;
        }
    }

    void ActivateInstructionsText()
    {
        InstrText.gameObject.SetActive(true);
        InstrText.text = "";

        InputActionMap intro = _controls.FindActionMap("Intro");
        intro.FindAction("W").performed += OnW;
        intro.FindAction("A").performed += OnA;
        intro.FindAction("S").performed += OnS;
        intro.FindAction("D").performed += OnD;
    }

    void EndInstructions()
    {
        InputActionMap intro = _controls.FindActionMap("Intro");
        intro.FindAction("W").performed -= OnW;
        intro.FindAction("A").performed -= OnA;
        intro.FindAction("S").performed -= OnS;
        intro.FindAction("D").performed -= OnD;
    }

    IEnumerator SpellTest()
    {
        ShowText("\\/[ ESPACIO ]\\/");

        Element.sprite = SymbolSprites[0];

        _spellModeTest = false;
        _spellTime = 0;
        while (!_spellModeTest)
        {
            ManageEnterSpell();
            yield return null;
        }
        yield return new WaitUntil(() => !Keyboard.current.spaceKey.isPressed);
        HideText();
        Element.color = new Color(1f, 1f, 1f, 1f);
        Element.DOFade(0f, 1f).Play();
        float oScale = Element.GetComponent<RectTransform>().localScale.x;
        Element.GetComponent<RectTransform>().DOScale(1.2f * oScale, 0.1f).Play().OnComplete(() => Element.GetComponent<RectTransform>().DOScale(0.8f * oScale, 1f).Play());
        yield return new WaitUntil(() => !Keyboard.current.spaceKey.isPressed);

        //Debug.Log("SPELL TEST COMPLETADO");
    }

    IEnumerator SpellTest2()
    {
        ShowText("\\/[ SHIFT ]\\/");

        Element.sprite = SymbolSprites[1];

        _spellModeTest = false;
        _spellTime = 0;
        while (!_spellModeTest)
        {
            ManageEnterSpell2();
            yield return null;
        }
        yield return new WaitUntil(() => !Keyboard.current.shiftKey.isPressed);
        HideText();
        Element.color = new Color(1f, 1f, 1f, 1f);
        Element.DOFade(0f, 1f).Play();
        float oScale = Element.GetComponent<RectTransform>().localScale.x;
        Element.GetComponent<RectTransform>().DOScale(1.2f * oScale, 0.1f).Play().OnComplete(() => Element.GetComponent<RectTransform>().DOScale(0.8f * oScale, 1f).Play());
        yield return new WaitUntil(() => !Keyboard.current.spaceKey.isPressed);

        //Debug.Log("SPELL TEST COMPLETADO");
    }

    #region Spell Instructions

    void OnW(InputAction.CallbackContext _)
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            _playerInstructions += "Up";
            InstrText.text += "W";
        }
    }

    void OnA(InputAction.CallbackContext _)
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            _playerInstructions += "Left";
            InstrText.text += "A";
        }
    }

    void OnS(InputAction.CallbackContext _)
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            _playerInstructions += "Down";
            InstrText.text += "S";
        }
    }

    void OnD(InputAction.CallbackContext _)
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            _playerInstructions += "Right";
            InstrText.text += "D";
        }
    }

    void GetRuneString()
    {
        if (ARune.FindSpell(Upgrades.StartElement.ToString(), out var fireSpell))
        {
            _runeInstructions = fireSpell.GetInstructionsToString();
        }
    }

    #endregion
}
