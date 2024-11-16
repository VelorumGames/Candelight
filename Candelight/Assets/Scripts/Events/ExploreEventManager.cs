using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Events
{
    public class ExploreEventManager : MonoBehaviour
    {
        public static ExploreEventManager Instance;

        MapManager _map;
        [SerializeField] GameObject[] _events;
        GameObject _currentEvent;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        public void GenerateEvent(MapManager map)
        {
            Debug.Log("Se inicia generacion de evento con ID: " + map.CurrentNodeInfo.EventID);
            _map = map;

            switch(_map.CurrentNodeInfo.EventID)
            {
                case 0:
                    ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();
                    room.RoomType = ERoomType.Event;
                    room.IdText.text += " EVENT";
                    room.gameObject.name = "Event Room";

                    _currentEvent = Instantiate(_events[0], room.GetRandomSpawnPoint());
                    break;
                default:
                    Debug.Log("No se generara ningun evento para este nodo");
                    break;
            }
        }

        public void LoadEventResult(EEventSolution sol)
        {
            _map.CurrentNodeInfo.EventSolution = sol;
        }
    }
}