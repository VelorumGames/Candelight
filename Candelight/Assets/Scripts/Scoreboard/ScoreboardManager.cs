using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        public GameObject Star;
        public Transform StarsContainer;
        GameObject[] _stars;

        string _currentName = "Prueba";

        [SerializeField] TextMeshProUGUI[] _bestPlayers;
        [SerializeField] TextMeshProUGUI[] _surroundingPlayers;

        private void Awake()
        {
            ResetPlayers();
        }

        void ResetPlayers()
        {
            foreach (var p in _bestPlayers) p.text = "";
            foreach (var p in _surroundingPlayers) p.text = "";
        }

        public void UpdatePlayers(UserData[] data)
        {
            SpawnStars(data);
            LoadBestPlayers(data);

            int index = 0;

            foreach (var d in data)
            {
                if (d.Name == _currentName) break;
                index++;
            }

            if (data.Length > 3 && index >= 3) LoadSurroundingPlayers(index, data);
        }

        void SpawnStars(UserData[] data)
        {
            _stars = new GameObject[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _stars[i] = Instantiate(Star, StarsContainer);
                _stars[i].GetComponent<Star>().LoadData(data[i]);
                _stars[i].transform.localPosition = new Vector2(data[i].posX, data[i].posY);
            }
        }

        void LoadBestPlayers(UserData[] data)
        {
            for (int i = 0; i < _bestPlayers.Length; i++)
            {
                _bestPlayers[i].text = $"{i + 1}. ({data[i].Score} - {data[i].Name})";
                if (data[i].Name == _currentName) _bestPlayers[i].color = Color.yellow;
            }
        }

        void LoadSurroundingPlayers(int index, UserData[] data)
        {
            index -= Math.Clamp(data.Length / 2, 0, 4);

            foreach(var p in _surroundingPlayers)
            {
                if (index > 0 && index < data.Length)
                {
                    p.text = $"{index + 1}. ({data[index].Score} - {data[index].Name})";
                    if (data[index].Name == _currentName) p.color = Color.yellow;
                    index++;
                }
            }
        }
    }
}