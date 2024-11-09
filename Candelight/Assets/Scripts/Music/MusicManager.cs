using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Music
{
    public class MusicManager : MonoBehaviour
    {
        AudioSource _source;

        Tween _lerpVolume;
        float _targetVolume;
        float _vLerpLength;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _lerpVolume = DOTween.To(() => _source.volume, x => _source.volume = x, _targetVolume, _vLerpLength).SetAutoKill(false);

            DontDestroyOnLoad(gameObject);
        }

        public void LoadClip(AudioClip newClip) => _source.clip = newClip;

        public void PlayMusic()
        {
            if (!_source.isPlaying) _source.Play();
        }

        public void PlayMusic(AudioClip musicClip)
        {
            _source.clip = musicClip;
            if (!_source.isPlaying) _source.Play();
        }

        public void StopMusic()
        {
            if (_source.isPlaying) _source.Stop();
        }

        public void ChangeVolumeFrom(float startVolume, float endVolume, float time)
        {
            _source.volume = startVolume;
            _targetVolume = endVolume;
            _vLerpLength = time;

            _lerpVolume.Restart();
            _lerpVolume.Play();
        }

        public void ChangeVolumeTo(float endVolume, float time)
        {
            _targetVolume = endVolume;
            _vLerpLength = time;

            _lerpVolume.Restart();
            _lerpVolume.Play();
        }
    }
}
