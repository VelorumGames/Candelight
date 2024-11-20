using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using World;

namespace Menu
{
    public class DeathWindow : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;

        private void Awake()
        {
            _world.OnPlayerDeath += ManageDeath;
        }

        void ManageDeath()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _world.OnPlayerDeath -= ManageDeath;
        }
    }
}
