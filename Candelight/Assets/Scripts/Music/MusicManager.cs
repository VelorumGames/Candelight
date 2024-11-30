using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Map;
using Dialogues;
using UnityEngine.Audio;
using Controls;

namespace Music
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] AudioSource _ambientSource;
        [SerializeField] AudioSource _exploreSource;
        [SerializeField] AudioSource _combatSource;
        [SerializeField] AudioSource _mainSource;
        AudioSource[] _sources;

        bool _randomMusic;

        [SerializeField] AudioMixer _mixer;
        InputManager _input;

        bool _inCombat;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }

        private void Awake()
        {
            _input = FindObjectOfType<InputManager>();

            _sources = new AudioSource[4];
            _sources[0] = _ambientSource;
            _sources[1] = _exploreSource;
            _sources[2] = _combatSource;
            _sources[3] = _mainSource;

            DontDestroyOnLoad(gameObject);
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            AuxiliarSpellModeReset();
        }

        void OnSceneUnload(Scene scene)
        {
            //Debug.Log("Quito las canciones");

            StopAllCoroutines();
            ChangeVolumeTo(0, 0f, 0.75f);
            ChangeVolumeTo(1, 0f, 0.75f);
            ChangeVolumeTo(2, 0f, 0.75f);
            ChangeVolumeTo(3, 0f, 0.75f);
        }

        public void LoadClip(int id, AudioClip newClip) => _sources[id].clip = newClip;
        public void LoadClips(AudioClip[] clips, float[] startVolumes)
        {
            if (clips.Length == 4 && startVolumes.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (clips[i] != null && startVolumes != null)
                    {
                        _sources[i].clip = clips[i];
                        ChangeVolumeTo(i, startVolumes[i], 1f);
                    }

                }
            }
            else Debug.LogWarning("ERROR: No se pueden cargar las canciones porque no coincide el tamano del array");
        }

        public void PlayMusic(int id)
        {
            if (!_sources[id].isPlaying) _sources[id].Play();
        }

        public void PlayAll()
        {
            foreach (var s in _sources) s.Play();
        }

        public void PlayMusicAtRandom(float minTime, float maxTime)
        {
            StopAllCoroutines();
            _randomMusic = true;
            StartCoroutine(RandomMusic(minTime, maxTime));
        }

        IEnumerator RandomMusic(float minTime, float maxTime)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            while (_randomMusic)
            {
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));

                _sources[1].loop = _inCombat;
                _sources[2].loop = _inCombat;

                if (!_sources[1].isPlaying)
                {
                    _sources[1].Play();
                    _sources[2].Play();

                    ChangeVolumeTo(1, 0.5f, 10f);
                }

                yield return new WaitUntil(() => !_sources[1].isPlaying);
            }
        }

        public void PlayMusic(int id, AudioClip musicClip)
        {
            _sources[id].clip = musicClip;
            if (!_sources[id].isPlaying) _sources[id].Play();
        }

        public void SetLooping(int id, bool b) => _sources[id].loop = b;

        public void StopMusic(int id)
        {
            if (_sources[id].isPlaying) _sources[id].Stop();
        }

        public void ChangeVolumeFrom(int id, float startVolume, float endVolume, float time)
        {
            _sources[id].volume = startVolume;
            _sources[id].DOFade(endVolume, time).Play().SetUpdate(true);
        }

        public void ChangeVolumeTo(int id, float endVolume, float time)
        {
            _sources[id].DOFade(endVolume, time).Play().SetUpdate(true);
        }

        public float GetCurrentVolume(int id) => _sources[id].volume;

        public void StartCombatMusic()
        {
            _inCombat = true;

            if (!_randomMusic) //Si no esta sonando ya la musica de exploracion
            {
                _sources[1].Play();
                _sources[2].Play();
            }

            ChangeVolumeTo(1, 0f, 2f);
            ChangeVolumeTo(2, 0.5f, 2f);
        }

        public void ReturnToExploreMusic()
        {
            _inCombat = false;

            ChangeVolumeTo(1, 0.5f, 2f);
            ChangeVolumeTo(2, 0f, 2f);
        }

        public void StartDialogueMusic()
        {
            for (int i = 1; i < 4; i++)
            {
                Debug.Log($"Compruebo volumen en {i}: {GetCurrentVolume(i)}");
                if (GetCurrentVolume(i) > 0.15f)
                {
                    Debug.Log($"Se baja la musica {i}");
                    ChangeVolumeTo(i, 0.15f, 1f);
                }
            }
        }

        public void EndDialogueMusic()
        {
            for (int i = 1; i < 4; i++)
            {
                if (GetCurrentVolume(i) < 0.3f && GetCurrentVolume(i) > 0.1f) ChangeVolumeTo(i, 0.5f, 2f);
            }
        }

        public void EnterSpellModeMusic()
        {
            _mixer.SetFloat("MusicCutoffHighFreq", 22000f);
            _mixer.DOSetFloat("MusicCutoffHighFreq", 1100f, 0.5f).SetUpdate(true).Play();

            _mixer.SetFloat("MusicPitch", 1f);
            _mixer.DOSetFloat("MusicPitch", 0.8f, 0.5f).SetUpdate(true).Play();
        }

        public void ExitSpellModeMusic()
        {
            _mixer.DOSetFloat("MusicCutoffHighFreq", 22000f, 0.5f).SetUpdate(true).Play();
            _mixer.DOSetFloat("MusicPitch", 1f, 0.5f).SetUpdate(true).Play();

            Invoke("AuxiliarSpellModeReset", 0.5f);
        }

        public void AuxiliarSpellModeReset()
        {
            if (!_input.IsInSpellMode())
            {
                _mixer.SetFloat("MusicCutoffHighFreq", 22000f);
                _mixer.SetFloat("MusicPitch", 1f);
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
    }
}
