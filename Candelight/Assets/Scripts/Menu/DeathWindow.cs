using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using World;

namespace Menu
{
    public class DeathWindow : MonoBehaviour
    {
        PlayerController _cont;

        private void Awake()
        {
            _cont = FindObjectOfType<PlayerController>();
            _cont.OnTruePlayerDeath += ManageDeath;
        }

        void ManageDeath()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _cont.OnTruePlayerDeath -= ManageDeath;
        }
    }
}
