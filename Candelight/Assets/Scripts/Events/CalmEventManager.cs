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

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        public void GenerateEvent(MapManager map)
        {
            _map = map;

            switch (_map.CurrentNodeInfo.EventID)
            {
                case 0:
                    ARoom room = _map.GetRandomAvailableRoom(true).GetComponent<ARoom>();
                    room.RoomType = ERoomType.Event;
                    room.IdText.text += " EVENT";

                    _currentEvent = Instantiate(_eventEndings[0], room.transform);
                    break;
                default:
                    Debug.Log("No se generara ningun evento para este nodo");
                    break;
            }
        }

        public EEventSolution GetEventSolution() => _map.CurrentNodeInfo.EventSolution;
    }
}
