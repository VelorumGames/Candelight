using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scoreboard
{
    public struct PlayerScoreData
    {
        public string Name;
        public int CompletedNodes;
        public Vector2 ScreenPos;
    }

    public class ScoreboardManager : MonoBehaviour
    {
        PlayerScoreData[] _playersData = new PlayerScoreData[20];
        public GameObject Star;
        public Transform StarsContainer;
        GameObject[] _stars;

        private void Awake()
        {
            _stars = new GameObject[_playersData.Length];
        }

        private void Start()
        {
            for (int i = 0; i < _playersData.Length; i++)
            {
                _playersData[i].Name = $"Player {i}";
                _playersData[i].CompletedNodes = Random.Range(0, 20);
                _playersData[i].ScreenPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            }

            SpawnStars();
        }

        void SpawnStars()
        {
            for (int i = 0; i < _playersData.Length; i++)
            {
                _stars[i] = Instantiate(Star, StarsContainer);
                _stars[i].GetComponent<Star>().LoadData(_playersData[i]);
                _stars[i].transform.localPosition = _playersData[i].ScreenPos;
            }
        }
    }
}