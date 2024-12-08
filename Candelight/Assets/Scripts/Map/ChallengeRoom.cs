using Items;
using Music;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class ChallengeRoom : ASimpleRoom
    {
        [SerializeField] GameObject[] _waves;
        int _waveCount;
        [SerializeField] int _fragReward;

        SimpleRoomManager _man;
        MusicManager _music;

        AudioSource _audio;

        private void Awake()
        {
            _man = FindObjectOfType<SimpleRoomManager>();
            _music = FindObjectOfType<MusicManager>();

            _audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            SpawnNextWave();

            _music.PlayMusic(2);
            _music.ChangeVolumeFrom(2, 0f, 0.5f, 3f);
        }

        public void SpawnNextWave()
        {
            if (_waveCount < _waves.Length) _waves[_waveCount++].SetActive(true); //Siguiente oleada
            else //Al finalizar el desafio
            {
                _music.ChangeVolumeTo(2, 0f, 5f);
                _audio.Play();

                FindObjectOfType<Inventory>().SpawnFragments(_fragReward, 1, GetRandomSpawn());
                _man.PlaceTorch(GetRandomSpawn());
            }
        }
    }
}
