using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class EndRoom : ASimpleRoom
    {
        private void Start()
        {
            FindObjectOfType<SimpleRoomManager>().PlaceTorch(GetRandomSpawn());
        }
    }
}