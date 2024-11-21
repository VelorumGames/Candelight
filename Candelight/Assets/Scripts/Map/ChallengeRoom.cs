using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class ChallengeRoom : ASimpleRoom
    {
        [SerializeField] GameObject[] _waves;
        int _waveCount;
        [SerializeField] int _fragReward;

        SimpleRoomManager _man;

        private void Awake()
        {
            _man = FindObjectOfType<SimpleRoomManager>();
        }

        private void Start()
        {
            SpawnNextWave();
        }

        public void SpawnNextWave()
        {
            if (_waveCount < _waves.Length) _waves[_waveCount++].SetActive(true); //Siguiente oleada
            else //Al finalizar el desafio
            {
                FindObjectOfType<Inventory>().SpawnFragments(_fragReward, 1, GetRandomSpawn());
                _man.PlaceTorch(GetRandomSpawn());
            }
        }
    }
}
