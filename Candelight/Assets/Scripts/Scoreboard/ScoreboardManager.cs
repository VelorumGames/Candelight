using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        public GameObject Star;
        public Transform StarsContainer;
        GameObject[] _stars;

        //private void Awake()
        //{
        //    _stars = new GameObject[_playersData.Length];
        //}
        //
        //private void Start()
        //{
        //    for (int i = 0; i < _playersData.Length; i++)
        //    {
        //        _playersData[i].Name = $"Player {i}";
        //        _playersData[i].CompletedNodes = Random.Range(0, 20);
        //        _playersData[i].ScreenPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        //    }
        //
        //    SpawnStars();
        //}

        public void SpawnStars(UserData[] data)
        {
            _stars = new GameObject[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _stars[i] = Instantiate(Star, StarsContainer);
                _stars[i].GetComponent<Star>().LoadData(data[i]);
                _stars[i].transform.localPosition = new Vector2(data[i].posX, data[i].posY);
            }
        }
    }
}