using Cameras;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        public GameObject Star;
        public Transform StarsContainer;
        GameObject[] _stars;

        [SerializeField] TextMeshProUGUI[] _bestPlayers;
        [SerializeField] TextMeshProUGUI[] _surroundingPlayers;

        CameraManager _cam;
        UIManager _ui;

        private void Awake()
        {
            _cam = FindObjectOfType<CameraManager>();
            _ui = FindObjectOfType<UIManager>();

            ResetPlayers();
        }

        private void Start()
        {
            if (SaveSystem.ScoreboardIntro)
            {
                _ui.FadeFromWhite(1f);
            }
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
                if (d.Name == SaveSystem.PlayerData.Name) break;
                index++;
            }

            if (data.Length > 3 && index >= 3) LoadSurroundingPlayers(index, data);
        }

        void SpawnStars(UserData[] data)
        {
            if (SaveSystem.PlayerData != null)
            {
                _stars = new GameObject[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    _stars[i] = Instantiate(Star, StarsContainer);
                    _stars[i].GetComponent<Star>().LoadData(data[i]);
                    _stars[i].transform.localPosition = new Vector2(data[i].posX, data[i].posY);

                    if (data[i].Name == SaveSystem.PlayerData.Name)
                    {
                        if (SaveSystem.ScoreboardIntro)
                        {
                            _cam.GetActiveCam().transform.position = new Vector3(data[i].posX, data[i].posY, _cam.GetActiveCam().transform.position.z); //Colocamos la camara en la estrella que pertenezca al jugador
                            _cam.SetActiveCamera(1, 5f);

                            SaveSystem.ScoreboardIntro = false;
                        }
                        else
                        {
                            _cam.SetActiveCamera(1, 0f);
                        }
                    }
                }
            }
            else Debug.LogWarning("ERROR: No se ha encontrado PlayerData en el sistema de guardado.");
        }

        void LoadBestPlayers(UserData[] data)
        {
            if (SaveSystem.PlayerData != null)
            {
                for (int i = 0; i < _bestPlayers.Length; i++)
                {
                    _bestPlayers[i].text = $"{i + 1}. ({data[i].Score} - {data[i].Name})";
                    if (data[i].Name == SaveSystem.PlayerData.Name) _bestPlayers[i].color = Color.yellow;
                }
            }
            else Debug.LogWarning("ERROR: No se ha encontrado PlayerData en el sistema de guardado.");
        }

        void LoadSurroundingPlayers(int index, UserData[] data)
        {
            index -= Math.Clamp(data.Length / 2, 0, 4);

            foreach(var p in _surroundingPlayers)
            {
                if (index > 0 && index < data.Length)
                {
                    p.text = $"{index + 1}. ({data[index].Score} - {data[index].Name})";
                    if (data[index].Name == SaveSystem.PlayerData.Name) p.color = Color.yellow;
                    index++;
                }
            }
        }
    }
}