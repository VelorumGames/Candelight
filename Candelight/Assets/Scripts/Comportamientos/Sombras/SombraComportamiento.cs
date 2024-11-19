using Hechizos;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Player;
using UnityEditor.Rendering;

namespace Comportamientos.Sombra
{
    public class SombraComportamiento : MonoBehaviour
    {

        //Variables:

        [SerializeField] float _rotateVelocity;

        private int _numSombras;
        private int _deaths;
        private int _sombrasdeaths 
        {
            get
            {
                return _deaths;
            }

            set
            {
                _deaths = value;

                if (value >= _numSombras)
                {
                    gameObject.SetActive(false);
                }
            }
        }


        private GameObject _sombraRing;

        [SerializeField] int _numRings = 0;

        private SombraRing ringSombraScript;
        private SombraIndividual individualSombraScript;
        private ASpell scASpell;

        [SerializeField] bool _rightDirection = true;

        [SerializeField] GameObject[] rings;
        

        [SerializeField] float approachDist;

        [SerializeField] float _timeSpawnNewRing;
        [SerializeField] int maxRings = 3;
        [SerializeField] float _ringsDistance;
        [SerializeField] float _incrementRingsDistance;

        private GameObject _player;

       


        //Array Prefabs Anillos


        //Array de GameObject de los anillos instanciados

        private void Awake()
        {

            ringSombraScript = GetComponent<SombraRing>();
            individualSombraScript = GetComponent<SombraIndividual>();


        }




        public bool EquipadoFuego() //Devuelve true si el jugador tiene equipado el elemento de fuego entre sus elementos equipados
        {
            foreach (AElementalRune elem in scASpell.Elements)
            {
                if (elem.Name == "Phantom")
                {
                    return true;
                }
            }
            return false;
        }



        public void Orbitar()
        {

            //Cada cierto tiempo se van a ir generando anillos de sombras concentricos
            Multiplicar();

            //Rotan alrededor del personaje
            //Cada circulo exterior tendra una velocidad menor o mayor de rotacion. Los circulos impares rotan en sentido contrario.

            _rotateVelocity = 30;

            for (int i = 0; i <= rings.Length; i++)
            {
                if (_numRings % 2 == 0)
                {
                    _rightDirection = true;
                }
                else
                {
                    _rightDirection = false;
                }

                _rotateVelocity = _rotateVelocity - 5;

                ringSombraScript.RingOrbitate(_rightDirection, _rotateVelocity);

            }



            //Si hacen contacto con colisiones de la sala, las atraviesan pero su sprite se vuelve mas transparente o de otro color (o una animacion en el que se vuelven negras)




            //El circulo o circulos de Sombras se va a ir estrechando

            approachDist = 0.001f * Time.deltaTime;
            individualSombraScript.Approach(approachDist);


            


        }

        public void Multiplicar()
        {
            StartCoroutine(ManageSombraMultiplying());
        }

        public void Alejarse()
        {
            //Animacion de enfado

            //Las Sombras se alejan
            approachDist = 2f;

            individualSombraScript.Approach(approachDist);


        }

        public void Disparar()
        {
            //Un disparo de fuego pero de color negro con borde rojo sale de cada child hacia el jugador
            individualSombraScript.Shoot();
        }

        public void AtaqueCuerpoACuerpo() //Funciona por contacto 
        {
            //El jugador recibe danno multiplicado por el numero de sombras del circulo

            //Y las Sombras de ese circulo desaparecen
        }

        IEnumerator ManageSombraMultiplying()
        {
            //Habrá un máximo de anillos que se puedan generar
            for (int i = 0; i < maxRings; i++)
            {
                //Meter aqui la animacion de aparicion de anillo de Sombras:

                if (i == 0)
                {
                    // Generar un child en un anillo exterior 
                    _sombraRing = Instantiate(_sombraRing, new Vector3(_player.transform.position.x, _player.transform.position.y, 0), Quaternion.identity);
                    _numRings++;
                    _numSombras += ringSombraScript._numRingSombras;
                    rings.Append(_sombraRing);
                    
                }
                else
                {
                    // Generar un child en un anillo exterior 
                    _sombraRing = Instantiate(_sombraRing, new Vector3(_sombraRing.transform.position.x + _ringsDistance, _sombraRing.transform.position.x + _ringsDistance, 0), Quaternion.identity);
                    _numRings++;
                    rings.Append(_sombraRing);
                    _numSombras += ringSombraScript._numRingSombras;
                    _ringsDistance += _incrementRingsDistance;
                    
                }

                //Hacer que sigan constantemente al jugador:
                _sombraRing.transform.SetParent(_player.transform);


                yield return new WaitForSeconds(_timeSpawnNewRing);

            }

        }
    }
}
