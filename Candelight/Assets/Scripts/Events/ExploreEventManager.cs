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

            ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();
            room.RoomType = ERoomType.Event;
            room.IdText.text += " EVENT";
            room.gameObject.name = "Event Room";

            switch (_map.CurrentNodeInfo.EventID)
            {
                case 0:
                    _currentEvent = Instantiate(_events[0], room.GetRandomSpawnPoint());
                    break;
                case 1: //Honor Herido
                    _currentEvent = Instantiate(_events[1], room.GetRandomSpawnPoint());
                    break;
                case 2: //Mr Bombastic
                    //TODO
                    //Si ya tiene 3 bombas en el inventario, pasar EventID a -1 y no generar el evento
                    _currentEvent = Instantiate(_events[2], room.GetRandomSpawnPoint());
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