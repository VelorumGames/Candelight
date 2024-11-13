using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Music
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] AudioSource _ambientSource;
        [SerializeField] AudioSource _exploreSource;
        [SerializeField] AudioSource _combatSource;
        [SerializeField] AudioSource _mainSource;
        AudioSource[] _sources;

        Tween _lerpAmbVolume;
        Tween _lerpExplVolume;
        Tween _lerpComVolume;
        Tween _lerpMainVolume;
        Tween[] _volumeLerps;

        float _targetVolume;
        float _vLerpLength;

        private void Awake()
        {
            _ambientSource = GetComponent<AudioSource>();
            _lerpAmbVolume = DOTween.To(() => _ambientSource.volume, x => _ambientSource.volume = x, _targetVolume, _vLerpLength).SetAutoKill(false);
            _lerpExplVolume = DOTween.To(() => _exploreSource.volume, x => _exploreSource.volume = x, _targetVolume, _vLerpLength).SetAutoKill(false);
            _lerpComVolume = DOTween.To(() => _combatSource.volume, x => _combatSource.volume = x, _targetVolume, _vLerpLength).SetAutoKill(false);
            _lerpMainVolume = DOTween.To(() => _mainSource.volume, x => _mainSource.volume = x, _targetVolume, _vLerpLength).SetAutoKill(false);

            _sources = new AudioSource[4];
            _sources[0] = _ambientSource;
            _sources[1] = _exploreSource;
            _sources[2] = _combatSource;
            _sources[3] = _mainSource;

            _volumeLerps = new Tween[4];
            _volumeLerps[0] = _lerpAmbVolume;
            _volumeLerps[1] = _lerpExplVolume;
            _volumeLerps[2] = _lerpComVolume;
            _volumeLerps[3] = _lerpMainVolume;

            DontDestroyOnLoad(gameObject);
        }

        public void LoadClip(int id, AudioClip newClip) => _sources[id].clip = newClip;

        public void PlayMusic(int id)
        {
            if (!_sources[id].isPlaying) _sources[id].Play();
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
            _targetVolume = endVolume;
            _vLerpLength = time;

            _volumeLerps[id].Restart();
            _volumeLerps[id].Play();
        }

        public void ChangeVolumeTo(int id, float endVolume, float time)
        {
            _targetVolume = endVolume;
            _vLerpLength = time;

            _volumeLerps[id].Restart();
            _volumeLerps[id].Play();
        }

        public float GetCurrentVolume(int id) => _sources[id].volume;
    }
}
