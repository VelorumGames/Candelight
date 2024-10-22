using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Map
{
    public class SimpleRoomManager : MonoBehaviour
    {
        public GameObject[] Rooms;
        GameObject _currentRoom;
        GameObject _player;

        public NodeInfo CurrentNodeInfo;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>().gameObject;
        }

        private void Start()
        {
            SpawnRoom();
        }

        void SpawnRoom()
        {
            _currentRoom = Instantiate(Rooms[Random.Range(0, Rooms.Length)]);
            _player.transform.position = _currentRoom.GetComponent<ASimpleRoom>().GetPlayerStart().position;
        }

        public void EndLevel()
        {
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
            }
        }
    }
}
