using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Comportamientos.Sombra
{

    public class SombraRing : MonoBehaviour
    {
        //Variables:

        

        [SerializeField] GameObject[] sombras;
        public SombraIndividual[] ScriptsSombras;

        public int _numRingSombras = 0;

        [SerializeField] EnemyInfo Info;

        public bool _rightDirection = true;

        // [SerializeField] int _sombrasSpawn;

        private void Awake()
        {
            _numRingSombras = ScriptsSombras.Length;
        }




        public void RingOrbitate(float _rotateVelocity)
        {
            if (_rightDirection)
            {
                transform.Rotate(0f, _rotateVelocity * Time.deltaTime, 0f);
            }
            else
            {
                transform.Rotate(0f, -(_rotateVelocity * Time.deltaTime), 0f);
            }



        }

    }
}
