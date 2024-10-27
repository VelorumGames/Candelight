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

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void OnEnable()
        {
            MapManager.Instance.OnRoomGenerationEnd += SpawnRewardNPC;
        }

        public void GenerateEvent(MapManager map)
        {
            _map = map;

            switch (_map.CurrentNodeInfo.EventID)
            {
                case 0:
                    Debug.Log("Se genera evento");
                    ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();
                    room.RoomType = ERoomType.Event;
                    room.IdText.text += " EVENT";
                    room.gameObject.name = "Event Room";

                    _currentEvent = Instantiate(_eventEndings[0], room.GetRandomSpawnPoint());
                    _currentEvent.transform.position = room.GetRandomSpawnPoint().position;
                    break;
                default:
                    Debug.Log("No se generara ningun evento para este nodo");
                    break;
            }
        }

        public EEventSolution GetEventSolution() => _map.CurrentNodeInfo.EventSolution;

        void SpawnRewardNPC()
        {
            if (_rewardEvents.Length > 0)
            {
                ARoom rewardRoom = MapManager.Instance.GetRandomAvailableRoom(true).GetComponent<ARoom>();
                rewardRoom.IdText.text += " REWARD";

                GameObject rewardNPC = Instantiate(_rewardEvents[Random.Range(0, _rewardEvents.Length)], rewardRoom.GetRandomSpawnPoint());
            }
        }

        private void OnDisable()
        {
            MapManager.Instance.OnRoomGenerationEnd -= SpawnRewardNPC;
        }
    }
}
