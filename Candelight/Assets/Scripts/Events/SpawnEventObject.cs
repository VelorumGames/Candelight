using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class SpawnEventObject : MonoBehaviour
    {
        public GameObject _eventObject;

        private void Start()
        {
            SpawnObject();
        }

        void SpawnObject()
        {
            GameObject room = FindObjectOfType<MapManager>().GetRandomAvailableRoom(false);
            Instantiate(_eventObject, room.GetComponent<ARoom>().GetRandomSpawnPoint());
        }
    }
}
