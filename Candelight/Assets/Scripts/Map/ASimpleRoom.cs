using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public abstract class ASimpleRoom : MonoBehaviour
    {
        [SerializeField] protected Transform _playerStart;
        [SerializeField] protected List<Transform> _spawnAnchors;

        public Transform GetPlayerStart() => _playerStart;

        protected Transform GetRandomSpawn()
        {
            Transform tr = _spawnAnchors[Random.Range(0, _spawnAnchors.Count)];
            _spawnAnchors.Remove(tr);
            return tr;
        }
    }

}