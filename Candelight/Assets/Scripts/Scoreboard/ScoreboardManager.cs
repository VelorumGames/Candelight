using Cameras;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UIElements;
using World;

namespace Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        public GameObject Star;
        public Transform StarsContainer;
        GameObject[] _stars;

        [SerializeField] TextMeshProUGUI[] _bestPlayers;
        [SerializeField] TextMeshProUGUI[] _surroundingPlayers;

        [SerializeField] WorldInfo _world;

        CameraManager _cam;
        UIManager _ui;

        AudioSource _audio;

        private void Awake()
        {
            _cam = FindObjectOfType<CameraManager>();
            _ui = FindObjectOfType<UIManager>();

            ResetPlayers();

            _audio = GetComponent<AudioSource>();
        }

        private IEnumerator Start()
        {
            if (SaveSystem.ScoreboardIntro)
            {
                if (_world.CompletedNodes > 0) _ui.FadeFromWhite(3f);
                else _ui.FadeFromBlack(3f);
                _audio.Play();
            }

            if (GameSettings.ScoreboardTutorial)
            {
                yield return new WaitForSeconds(1.5f);
                _ui.ShowTutorial("Cada estrella del firmamento es otro mundo. Haz click en ellas para ver su progreso.");
                GameSettings.ScoreboardTutorial = false;
            }
        }

        void ResetPlayers()
        {
            foreach (var p in _bestPlayers) p.text = "";
            foreach (var p in _surroundingPlayers) p.text = "";
        }

        public void UpdatePlayers(ScoreData[] data)
        {
            SpawnStars(data);
            LoadBestPlayers(data);

            if (SaveSystem.ScoreboardData != null)
            {
                int index = 0;

                foreach (var d in data)
                {
                    if (d.Name == SaveSystem.ScoreboardData.Name) break;
                    index++;
                }

                if (data.Length > 3 && index >= 3) LoadSurroundingPlayers(index, data);
            }
            else Debug.LogWarning("ERROR: No se han encontrado datos del jugador en el sistema");
        }

        void SpawnStars(ScoreData[] data)
        {
            if (SaveSystem.ScoreboardData != null)
            {
                _stars = new GameObject[data.Length];

                Vector3 camPosition = new Vector3(-999f, -999f, -999f);

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Score > 0)
                    {
                        _stars[i] = Instantiate(Star, StarsContainer);
                        _stars[i].GetComponent<Star>().LoadData(data[i]);
                        _stars[i].transform.localPosition = new Vector2(data[i].posX, data[i].posY);

                        if (data[i].Name == SaveSystem.ScoreboardData.Name)
                        {
                            camPosition = new Vector3(data[i].posX, data[i].posY, _cam.GetActiveCam().transform.position.z); //Colocamos la camara en la estrella que pertenezca al jugador
                        }
                    }
                }

                if (SaveSystem.ScoreboardIntro && camPosition.x != -999f)
                {
                    _cam.GetActiveCam().transform.position = camPosition;
                    _cam.SetActiveCamera(1, 5f);

                    SaveSystem.ScoreboardIntro = false;
                }
                else
                {
                    _cam.SetActiveCamera(1, 0f);
                }
                
            }
            else Debug.LogWarning("ERROR: No se ha encontrado PlayerData en el sistema de guardado.");
        }

        void LoadBestPlayers(ScoreData[] data)
        {
            for (int i = 0; i < _bestPlayers.Length; i++)
            {
                if (i < data.Length)
                {
                    _bestPlayers[i].text = $"{i + 1}. {data[i].Name}: {data[i].Score} pts";
                    if (data[i].Name == SaveSystem.ScoreboardData.Name) _bestPlayers[i].color = Color.yellow;
                    else _bestPlayers[i].color = Color.white;
                }
            }
        }

        void LoadSurroundingPlayers(int index, ScoreData[] data)
        {
            index -= Math.Clamp(data.Length / 2, 0, 4);

            foreach(var p in _surroundingPlayers)
            {
                if (index > 0 && index < data.Length)
                {
                    p.text = $"{index + 1}. ({data[index].Score} - {data[index].Name})";
                    if (data[index].Name == SaveSystem.ScoreboardData.Name) p.color = Color.yellow;
                    else p.color = Color.white;
                    index++;
                }
            }
        }
    }
}