using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class TutorialRoom : ASimpleRoom
    {
        PlayerController _player;
        [SerializeField] GameObject _endTorch;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            _player.transform.position = GetPlayerStart().position;
        }

        public void SpawnEnd()
        {
            Instantiate(_endTorch, GetRandomSpawn());
        }
    }
}