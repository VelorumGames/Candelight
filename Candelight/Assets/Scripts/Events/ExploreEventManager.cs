using Items;
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

        [SerializeField] GameObject[] _durniaEvents;
        [SerializeField] GameObject[] _temeriaEvents;
        [SerializeField] GameObject[] _idriaEvents;

        GameObject[] _events;
        GameObject _currentEvent;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            switch(GetComponent<MapManager>().CurrentNodeInfo.Biome)
            {
                case EBiome.Durnia:
                    _events = _durniaEvents;
                    break;
                case EBiome.Temeria:
                    _events = _temeriaEvents;
                    break;
                case EBiome.Idria:
                    _events = _idriaEvents;
                    break;
            }
        }

        public void GenerateEvent(MapManager map)
        {
            Debug.Log("Se inicia generacion de evento con ID: " + map.CurrentNodeInfo.EventID);
            _map = map;

            ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();

            switch (_map.CurrentNodeInfo.EventID)
            {
                case 0:
                    _currentEvent = Instantiate(_events[0], room.GetRandomSpawnPoint());
                    break;
                case 1: //Honor Herido
                    _currentEvent = Instantiate(_events[1], room.GetRandomSpawnPoint());
                    break;
                case 2: //Mr Bombastic
                    //Si ya tiene 3 bombas en el inventario, pasar EventID a -1 y no generar el evento
                    Debug.Log("EL JUGADOR YA TIENE 3 BOMBAS ASI QUE SE DESACTIVA EL EVENTO");
                    if (FindObjectOfType<Inventory>().FindItem("Bomba de Pólvora", out var n) && n >= 3)
                    {
                        _map.CurrentNodeInfo.EventID = -1;
                        return;
                    }

                    _currentEvent = Instantiate(_events[2], room.GetRandomSpawnPoint());
                    break;
                default:
                    Debug.Log("No se generara ningun evento para este nodo");
                    break;
            }

            room.RoomType = ERoomType.Event;
            room.IdText.text += " EVENT";
            room.gameObject.name = "Event Room";
        }

        public void LoadEventResult(EEventSolution sol)
        {
            _map.CurrentNodeInfo.EventSolution = sol;
        }
    }
}