using Controls;
using Music;
using Player;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Map
{
    public class SimpleRoomManager : MonoBehaviour
    {
        public GameObject[] DurniaRooms;
        public GameObject[] TemeriaRooms;
        public GameObject[] IdriaRooms;
        GameObject _currentRoom;
        GameObject _player;
        [SerializeField] GameObject _endTorch;

        public NodeInfo CurrentNodeInfo;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>().gameObject;

            FindObjectOfType<InputManager>().LoadControls(EControlMap.Level);
        }

        private void Start()
        {
            SpawnRoom();
        }

        void SpawnRoom()
        {
            switch(CurrentNodeInfo.Biome)
            {
                case EBiome.Durnia:
                    _currentRoom = Instantiate(DurniaRooms[Random.Range(0, DurniaRooms.Length)]);
                    break;
                case EBiome.Temeria:
                    _currentRoom = Instantiate(TemeriaRooms[Random.Range(0, TemeriaRooms.Length)]);
                    break;
                case EBiome.Idria:
                    _currentRoom = Instantiate(IdriaRooms[Random.Range(0, IdriaRooms.Length)]);
                    break;
                default:
                    Debug.LogWarning("ERROR: No se ha registrado bien el bioma. Se defaultea a Durnia.");
                    _currentRoom = Instantiate(DurniaRooms[Random.Range(0, DurniaRooms.Length)]);
                    break;
            }
            
            _player.transform.position = _currentRoom.GetComponent<ASimpleRoom>().GetPlayerStart().position;
        }

        public void PlaceTorch(Transform tr)
        {
            Instantiate(_endTorch, tr);
        }

        public void EndLevel()
        {
            Debug.Log($"Ultimo nivel: {CurrentNodeInfo.CurrentLevel < CurrentNodeInfo.Levels - 1}");
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
            if (CurrentNodeInfo.CurrentLevel < CurrentNodeInfo.Levels - 1) //Si no es el ultimo nivel todavia
            {
                //Se apunta a la siguiente seed y se elije un tipo de nivel al que ir
                switch (CurrentNodeInfo.LevelTypes[++CurrentNodeInfo.CurrentLevel])
                {
                    case ELevel.Exploration:
                        SceneManager.LoadScene("LevelScene");
                        break;
                    case ELevel.Calm:
                        SceneManager.LoadScene("CalmScene");
                        break;
                    case ELevel.Challenge:
                        SceneManager.LoadScene("ChallengeScene");
                        break;
                }
            }
            else //Si es el ultimo nivel
            {
                //Se vuelve al mapa del mundo
                SceneManager.LoadScene("NodeEndScene");
                //CurrentNodeInfo.Node.RegisterCompletedNode();
            }
        }
    }
}
