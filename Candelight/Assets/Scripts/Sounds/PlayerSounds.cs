using Controls;
using DG.Tweening;
using Hechizos.DeForma;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

public class PlayerSounds : MonoBehaviour
{
    public NodeInfo CurrentNodeInfo;

    public AudioClip[] Book; //0: Open, 1: Close, 2: Acierto
    public AudioClip[] BookWriting;
    [Space(10)]
    public AudioClip[] Elements; //0: Fire, 1: Electric, 2: Cosmic, 3: Phantom
    public AudioClip[] DurniaFootsteps;
    public AudioClip[] TemeriaFootsteps;
    public AudioClip[] IdriaFootsteps;
    AudioClip[] _footsteps;
    [Space(10)]
    public AudioClip[] SpellMode; //0: Enter element, //1: Enter shape, 2: Stay, 3: Exit, 4: Successful
    public AudioClip[] SuccessSpell;
    public AudioClip[] RuneWriting;
    [Space(10)]
    public AudioClip Damage;
    public AudioClip DeathSound;
    public AudioClip ReviveSound;

    [SerializeField] AudioSource _audio;
    [SerializeField] AudioSource _constAudio;
    [SerializeField] AudioSource _stepAudio;
    [SerializeField] AudioSource _damAudio;
    [SerializeField] AudioSource _deathAudio;

    InputManager _input;
    PlayerController _player;

    bool _bookOpen;

    private void Awake()
    {
        _input = FindObjectOfType<InputManager>();
        _player = GetComponentInParent<PlayerController>();

        _constAudio.clip = SpellMode[2];
        _constAudio.Play();

        LoadBiomeSounds();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;

        _player.OnNewInstruction += PlayRuneSound;
        _player.OnSpell += ManageSpellExit;

        _input.OnStartElementMode += PlayStartElement;
        _input.OnStartShapeMode += PlayStartShape;
        _input.OnExitElementMode += PlayExitSpell;
    }

    void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "IntroScene") _stepAudio.volume = 0.1f;
        else _stepAudio.volume = 0.3f;
    }

    public void PlayDeathSound() => _audio.PlayOneShot(DeathSound);
    public void PlayReviveSound() => _audio.PlayOneShot(ReviveSound);

    public void PlayBookSound()
    {
        if (_bookOpen)
        {
            _audio.PlayOneShot(Book[1]);
            _bookOpen = false;
        }
        else
        {
            _audio.PlayOneShot(Book[0]);
            _bookOpen = true;
        }
    }

    public void PlayBookSuccess()
    {
        _audio.PlayOneShot(Book[2]);
    }

    public void PlayRuneSound(ESpellInstruction rune)
    {
        _audio.PlayOneShot(_bookOpen ? BookWriting[Random.Range(0, BookWriting.Length)] : RuneWriting[Random.Range(0, RuneWriting.Length)]);
    }

    public void PlayElement(string name)
    {
        switch(name)
        {
            case "Fire":
                _audio.PlayOneShot(Elements[0]);
                break;
            case "Electric":
                _audio.PlayOneShot(Elements[1]);
                break;
            case "Cosmic":
                _audio.PlayOneShot(Elements[2]);
                break;
            case "Phantom":
                _audio.PlayOneShot(Elements[3]);
                break;
        }
    }

    void PlayStartElement()
    {
        _audio.PlayOneShot(SpellMode[0]);
        _constAudio.volume = 0.3f;
    }
    void PlayStartShape()
    {
        _audio.PlayOneShot(SpellMode[1]);
        _constAudio.volume = 0.3f;
    }
    void PlayExitSpell()
    {
        _audio.PlayOneShot(SpellMode[3]);
        _constAudio.volume = 0f;
    }

    void PlaySuccesfulSpell()
    {
        _audio.PlayOneShot(SuccessSpell[Random.Range(0, SuccessSpell.Length)]);
        _constAudio.volume = 0f;
    }

    void ManageSpellExit(AShapeRune spell)
    {
        if (spell != null) PlaySuccesfulSpell();
        else PlayExitSpell();
    }

    public void PlayFootstepSound()
    {
        _stepAudio.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
    }

    public void PlayDamage()
    {
        _damAudio.pitch = Random.Range(0.75f, 1.25f);
        _damAudio.PlayOneShot(Damage);
    }

    void LoadBiomeSounds()
    {
        switch(CurrentNodeInfo.Biome)
        {
            case EBiome.Durnia:
                _footsteps = DurniaFootsteps;
                break;
            case EBiome.Temeria:
                _footsteps = TemeriaFootsteps;
                break;
            case EBiome.Idria:
                _footsteps = IdriaFootsteps;
                break;
            default:
                _footsteps = DurniaFootsteps;
                break;
        }
    }

    public void StartDeathAudio()
    {
        _deathAudio.DOFade(0.3f, 10f);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;

        _player.OnNewInstruction -= PlayRuneSound;
        _player.OnSpell -= ManageSpellExit;

        _input.OnStartElementMode -= PlayStartElement;
        _input.OnStartShapeMode -= PlayStartShape;
        _input.OnExitElementMode -= PlayExitSpell;
    }
}
