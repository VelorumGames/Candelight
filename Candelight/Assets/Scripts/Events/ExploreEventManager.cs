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

        Inventory _inv;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _inv = FindObjectOfType<Inventory>();
            _map = GetComponent<MapManager>();
        }

        private void Start()
        {
            LoadEventResult(EEventSolution.Ignored);

            switch(_map.CurrentNodeInfo.Biome)
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

        public void GenerateEvent()
        {
            Debug.Log("Se inicia generacion de evento con ID: " + _map.CurrentNodeInfo.EventID);

            ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();

            switch(GetComponent<MapManager>().CurrentNodeInfo.Biome)
            {
                // === DURNIA ===
                case EBiome.Durnia:
                    switch (_map.CurrentNodeInfo.EventID)
                    {
                        /*case 0: //Sepultado
                            _currentEvent = Instantiate(_events[0], room.GetRandomSpawnPoint());
                            break;*/
                        case 1: //Honor Herido
                            _currentEvent = Instantiate(_events[1], room.GetRandomSpawnPoint());
                            break;
                        case 2: //Mr Bombastic
                            //Si ya tiene 3 bombas en el inventario, pasar EventID a -1 y no generar el evento
                            if (_inv.FindItem("Bomba de Pólvora", out int n) && n >= 3)
                            {
                                Debug.Log("EL JUGADOR YA TIENE 3 BOMBAS ASI QUE SE DESACTIVA EL EVENTO");
                                _map.CurrentNodeInfo.EventID = -1;
                                return;
                            }

                            _currentEvent = Instantiate(_events[2], room.GetRandomSpawnPoint());
                            break;
                        default:
                            Debug.Log("No se generara ningun evento para este nodo");
                            break;
                    }
                    break;

                // === TEMERIA ===
                case EBiome.Temeria:
                    switch (_map.CurrentNodeInfo.EventID)
                    {
                        case 0: //Monstruo prisionero
                            //Si ya tiene 3 munecas en el inventario, pasar EventID a -1 y no generar el evento
                            if (_inv.FindItem("Muñeca de Temerio", out int n) && n >= 3)
                            {
                                Debug.Log("EL JUGADOR YA TIENE 3 MUÑECAS ASI QUE SE DESACTIVA EL EVENTO");
                                _map.CurrentNodeInfo.EventID = -1;
                                return;
                            }

                            if (!(room is EnemyRoom)) _currentEvent = Instantiate(_events[0], room.GetRandomSpawnPoint());
                            break;
                        case 1: //Sepultado
                            _currentEvent = Instantiate(_events[1], room.GetRandomSpawnPoint());
                            break;
                        default:
                            Debug.Log("No se generara ningun evento para este nodo");
                            break;
                    }
                    break;

                // === IDRIA ===
                case EBiome.Idria:
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