using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class ChallengeRoom : ASimpleRoom
    {
        [SerializeField] GameObject[] _waves;
        int _waveCount;

        private void Start()
        {
            SpawnNextWave();
        }

        public void SpawnNextWave()
        {
            if (_waveCount < _waves.Length) _waves[_waveCount++].SetActive(true);
            else FindObjectOfType<SimpleRoomManager>().EndLevel();
        }
    }
}
