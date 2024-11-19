using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Comportamientos.Sombra
{

    public class SombraRing : MonoBehaviour
    {
        //Variables:

        private float _ringRadius;

         [SerializeField] GameObject[] sombras;

        public int _numRingSombras = 0;

        // [SerializeField] int _sombrasSpawn;

        private void Awake()
        {
            _ringRadius = 20;
        }

        public int CountSombrasInRing()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _numRingSombras++;

            }

            return _numRingSombras;
        }

        public void RingOrbitate(bool rightDirection, float _rotateVelocity)
        {
            if (rightDirection)
            {
                transform.Rotate(0f, 0f, _rotateVelocity * Time.deltaTime);
            }
            else
            {
                transform.Rotate(0f, 0f, -(_rotateVelocity * Time.deltaTime));
            }



        }

        public float GetRadius() { return _ringRadius; }
        public void SetRadius(float rad) { _ringRadius = rad; }

    }
}
