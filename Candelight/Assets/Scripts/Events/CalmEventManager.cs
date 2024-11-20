using Dialogues;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Events
{
    public class CalmEventManager : MonoBehaviour
    {
        public static CalmEventManager Instance;

        MapManager _map;
        [SerializeField] GameObject[] _eventEndings;
        GameObject _currentEvent;

        [SerializeField] GameObject[] _rewardEvents;

        [Space(10)]
        [SerializeField] GameObject _healthAltar;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void OnEnable()
        {
            MapManager.Instance.OnRoomGenerationEnd += SpawnRewardNPC;
            MapManager.Instance.OnRoomGenerationEnd += SpawnHealthAltar;
        }

        public void GenerateEvent(MapManager map)
        {
            _map = map;

            Debug.Log("Se genera evento");
            ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();
            room.RoomType = ERoomType.Event;
            room.IdText.text += " EVENT";
            room.gameObject.name = "Event Room";

            switch (_map.CurrentNodeInfo.EventID)
            {
                case 0: //Test
                    _currentEvent = Instantiate(_eventEndings[0], room.GetRandomSpawnPoint());
                    break;
                case 1: //Honor Herido
                    switch(GetEventSolution())
                    {
                        case EEventSolution.Ignored:
                            _currentEvent = Instantiate(_eventEndings[1], room.GetRandomSpawnPoint());
                            break;
                        case EEventSolution.Failed:
                            _currentEvent = Instantiate(_eventEndings[1], room.GetRandomSpawnPoint());
                            break;
                        case EEventSolution.Completed:
                            _currentEvent = Instantiate(_eventEndings[2], room.GetRandomSpawnPoint());
                            break;
                    }
                    break;
                case 2: //Mr Bombastic
                    break;
                default:
                    Debug.Log("No se generara ningun evento para este nodo");
                    break;
            }

            if (_currentEvent != null) _currentEvent.transform.position = room.GetRandomSpawnPoint().position;
        }

        public EEventSolution GetEventSolution() => _map.CurrentNodeInfo.EventSolution;

        void SpawnRewardNPC()
        {
            if (_rewardEvents.Length > 0)
            {
                ARoom rewardRoom = MapManager.Instance.GetRandomAvailableRoom(true).GetComponent<ARoom>();
                rewardRoom.IdText.text += " REWARD";

                Instantiate(_rewardEvents[Random.Range(0, _rewardEvents.Length)], rewardRoom.GetRandomSpawnPoint());
            }
        }

        void SpawnHealthAltar()
        {
            ARoom healthRoom = MapManager.Instance.GetRandomAvailableRoom(true).GetComponent<ARoom>();
            Instantiate(_healthAltar, healthRoom.GetRandomSpawnPoint());
        }

        private void OnDisable()
        {
            MapManager.Instance.OnRoomGenerationEnd -= SpawnRewardNPC;
            MapManager.Instance.OnRoomGenerationEnd -= SpawnHealthAltar;
        }
    }
}
