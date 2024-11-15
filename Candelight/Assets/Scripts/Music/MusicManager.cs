using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Map;

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

        MapManager _map;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }

        private void Awake()
        {
            _sources = new AudioSource[4];
            _sources[0] = _ambientSource;
            _sources[1] = _exploreSource;
            _sources[2] = _combatSource;
            _sources[3] = _mainSource;

            DontDestroyOnLoad(gameObject);
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            _map = FindObjectOfType<MapManager>();

            if (_map)
            {
                _map.OnCombatStart += StartCombatMusic;
                _map.OnCombatEnd += ReturnToExploreMusic;
            }
        }

        void OnSceneUnload(Scene scene)
        {
            if (_map)
            {
                _map.OnCombatStart -= StartCombatMusic;
                _map.OnCombatEnd -= ReturnToExploreMusic;
            }
        }

        public void LoadClip(int id, AudioClip newClip) => _sources[id].clip = newClip;
        public void LoadClips(AudioClip[] clips, float[] startVolumes)
        {
            if (clips.Length == 4 && startVolumes.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    _sources[i].clip = clips[i];
                    ChangeVolumeTo(i, startVolumes[i], 1f);
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

                if (!_sources[1].isPlaying)
                {
                    _sources[1].Play();
                    _sources[2].Play();
                }
            }
        }

        public void PlayMusic(int id, AudioClip musicClip)
        {
            _sources[id].clip = musicClip;
            if (!_sources[id].isPlaying) _sources[id].Play();
        }

        public void StopMusic(int id)
        {
            if (_sources[id].isPlaying) _sources[id].Stop();
        }

        public void ChangeVolumeFrom(int id, float startVolume, float endVolume, float time)
        {
            _sources[id].volume = startVolume;
            _sources[id].DOFade(endVolume, time).Play();
        }

        public void ChangeVolumeTo(int id, float endVolume, float time)
        {
            _sources[id].DOFade(endVolume, time).Play();
        }

        public float GetCurrentVolume(int id) => _sources[id].volume;

        void StartCombatMusic()
        {
            if (!_randomMusic) //Si no esta sonando ya la musica de exploracion
            {
                _sources[1].Play();
                _sources[2].Play();
            }

            ChangeVolumeTo(1, 0f, 2f);
            ChangeVolumeTo(2, 0.5f, 2f);
        }

        void ReturnToExploreMusic()
        {
            ChangeVolumeTo(1, 0.5f, 2f);
            ChangeVolumeTo(2, 0f, 2f);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
    }
}
