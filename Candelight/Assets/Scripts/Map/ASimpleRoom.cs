using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public abstract class ASimpleRoom : MonoBehaviour
    {
        [SerializeField] protected Transform _playerStart;

        public Transform GetPlayerStart() => _playerStart;
    }

}