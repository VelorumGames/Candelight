using Controls;
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

    public AudioClip[] Book; //0: Open, 1: Close
    public AudioClip[] BookWriting;
    [Space(10)]
    public AudioClip[] Elements; //0: Fire, 1: Electric, 2: Cosmic, 3: Phantom
    public AudioClip[] DurniaFootsteps;
    public AudioClip[] TemeriaFootsteps;
    public AudioClip[] IdriaFootsteps;
    AudioClip[] _footsteps;
    [Space(10)]
    public AudioClip[] SpellMode; //0: Enter element, //1: Enter shape, 2: Stay, 3: Exit, 4: Successful
    public AudioClip[] RuneWriting;
    [Space(10)]
    public AudioClip DeathSound;
    public AudioClip ReviveSound;

    [SerializeField] AudioSource _audio;
    [SerializeField] AudioSource _constAudio;

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
        _player.OnNewInstruction += PlayRuneSound;
        _player.OnSpell += ManageSpellExit;

        _input.OnStartElementMode += PlayStartElement;
        _input.OnStartShapeMode += PlayStartShape;
        _input.OnExitElementMode += PlayExitSpell;
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

    void PlayRuneSound(ESpellInstruction rune)
    {
        _audio.PlayOneShot(_bookOpen ? BookWriting[Random.Range(0, BookWriting.Length)] : RuneWriting[Random.Range(0, RuneWriting.Length)]);
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
        _audio.PlayOneShot(SpellMode[4]);
        _constAudio.volume = 0f;
    }

    void ManageSpellExit(AShapeRune spell)
    {
        if (spell != null) PlaySuccesfulSpell();
        else PlayExitSpell();
    }

    public void PlayFootstepSound()
    {
        _audio.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
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
        }
    }

    private void OnDisable()
    {
        _player.OnNewInstruction -= PlayRuneSound;
        _player.OnSpell -= ManageSpellExit;

        _input.OnStartElementMode -= PlayStartElement;
        _input.OnStartShapeMode -= PlayStartShape;
        _input.OnExitElementMode -= PlayExitSpell;
    }
}
