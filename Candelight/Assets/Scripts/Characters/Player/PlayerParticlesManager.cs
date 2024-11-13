using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerParticlesManager : MonoBehaviour
    {
        [SerializeField] ParticleSystem _ambientParticles;
        [SerializeField] ParticleSystem _footParticles;

        bool _footPlayed;

        private void Start()
        {
            _footParticles.Stop();
        }

        public void StartFootParticles()
        {
            if (!_footPlayed)
            {
                Debug.Log("Se comienzan las particulas");
                _footParticles.Play();
                _footPlayed = true;
            }
        }

        public void StopFootParticles()
        {
            if (_footPlayed)
            {
                Debug.Log("Se paran las particulas");
                _footParticles.Stop();
                _footPlayed = false;
            }
        }
    }
}
