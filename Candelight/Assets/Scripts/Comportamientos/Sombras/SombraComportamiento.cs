using Animations;
using Enemy;
using Hechizos;
using Hechizos.Elementales;
using Player;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Comportamientos.Sombra
{
    public class SombraComportamiento : MonoBehaviour
    {

        //Variables:

        [SerializeField] float _rotateVelocity;

        private int _numSombras;
        private int _deaths;
        public int _sombrasdeaths
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
                    _audio.PlayOneShot(FinalDeath);
                    StartCoroutine(Muerte());
                    //gameObject.SetActive(false);
                }
            }
        }

        private List<SombraIndividual> _sombrasIndividualesScripts = new List<SombraIndividual>();

        [SerializeField] GameObject _sombraRing;

        [SerializeField] int _numRings = 0;

        [SerializeField] EnemyInfo Info;


        [SerializeField] List<SombraRing> rings = new List<SombraRing>();


        private float approachDist;

        [SerializeField] float _timeSpawnNewRing;
        [SerializeField] int maxRings = 3;
        [SerializeField] float _ringsDistance;
        [SerializeField] float _incrementRingsDistance;

        private GameObject _player;
        private float vel;

        List<SombraAnimation> _anims = new List<SombraAnimation>();
        UIManager _ui;

        [SerializeField] AudioSource _enfadoSource;
        [SerializeField] AudioSource _audio;

        public AudioClip Calma;
        public AudioClip FinalDeath;

        //Array Prefabs Anillos

        //Array de GameObject de los anillos instanciados

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>().gameObject;
            _ui = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            //Cada cierto tiempo se van a ir generando anillos de sombras concentricos
            Multiplicar();
        }

        private void Update()
        {
            Orbitar();
        }


        public bool EquipadoFuego() //Devuelve true si el jugador tiene equipado el elemento de fuego entre sus elementos equipados
        {
            foreach (AElementalRune elem in ARune.MageManager.GetActiveElements())
            {
                if (elem.Name == "Fire")
                {
                    return true;
                }
            }
            return false;
        }



        public void Orbitar()
        {

            approachDist = 0.009f * Time.deltaTime + 1f;
            vel = _rotateVelocity;

            //Rotan alrededor del personaje
            //Cada circulo exterior tendra una velocidad menor o mayor de rotacion. Los circulos impares rotan en sentido contrario.
            //El circulo o circulos de Sombras se va a ir estrechando

            for (int i = 0; i < rings.Count; i++)
            {

                for (int j = 0; j < rings[i].ScriptsSombras.Length; j++)
                {
                    if (rings[i].ScriptsSombras[j] != null)
                    {
                        rings[i].ScriptsSombras[j].Approach(approachDist);
                    }
                    
                }
                

                vel -= 2;

                rings[i].RingOrbitate(vel);

            }

            //Si hacen contacto con colisiones de la sala, las atraviesan pero su sprite se vuelve mas transparente o de otro color (o una animacion en el que se vuelven negras)
        }

        public void Multiplicar()
        {
            StartCoroutine(ManageSombraMultiplying());
        }

        public void Alejarse()
        {
            //Animacion de enfado

            //Las Sombras se alejan

            for (int i = 0; i < _sombrasIndividualesScripts.Count; i++)
            {
                _sombrasIndividualesScripts[i].GoAway(2f);
            }
        }

        public void Disparar()
        {
            //Un disparo de fuego pero de color negro con borde rojo sale de cada child hacia el jugador
           /* for (int i = 0; i < _sombrasIndividualesScripts.Count; i++)
            {
                _sombrasIndividualesScripts[i].Shoot();
            }
           */

        }

        IEnumerator Muerte()
        {
            transform.localScale = new Vector3();
            yield return new WaitForSeconds(2f);

            foreach (SombraRing ring in rings)
            {
                Destroy(ring.gameObject);
            }

            Destroy(gameObject);
        }


        IEnumerator ManageSombraMultiplying()
        {
            //Habrá un máximo de anillos que se puedan generar
            for (int i = 0; i < maxRings; i++)
            {
                //Meter aqui la animacion de aparicion de anillo de Sombras:


                // Generar un child en un anillo exterior 
                GameObject mySombraRing = Instantiate(_sombraRing, _player.transform);
                _numRings++;
                _numSombras += mySombraRing.GetComponent<SombraRing>()._numRingSombras;
                rings.Add(mySombraRing.GetComponent<SombraRing>());
                // mySombraRing.transform.parent = _player.transform;


                foreach (var scSombra in mySombraRing.GetComponent<SombraRing>().ScriptsSombras)
                {
                    _sombrasIndividualesScripts.Add(scSombra);
                    _anims.Add(scSombra.GetComponent<SombraAnimation>());

                    scSombra._scSombracomportamiento = this;

                    scSombra.transform.localPosition = new Vector3(scSombra.transform.localPosition.x * _ringsDistance * (i+1), scSombra.transform.localPosition.y, scSombra.transform.localPosition.z * _ringsDistance * (i + 1));

                    if (EquipadoFuego())
                    {
                        _ui.ShowTutorial("\"Las sombras chillaron...\"", 3f);
                        scSombra.GoAway(2f);

                        _enfadoSource.volume = 0.3f;
                    }
                    else
                    {
                        _enfadoSource.volume = 0f;
                        if (_audio.isPlaying)
                        {
                            _audio.clip = Calma;
                            _audio.Play();
                        }
                    }
                   
                }

                if (_numRings % 2 == 0)
                {
                    mySombraRing.GetComponent<SombraRing>()._rightDirection = true;
                }
                else
                {
                    mySombraRing.GetComponent<SombraRing>()._rightDirection = false;
                }


                yield return new WaitForSeconds(_timeSpawnNewRing - 2f);

                if (_anims.Count > 0)
                {
                    foreach(var anim in _anims)
                    {
                        anim.ChangeToDivide();
                    }
                }

                yield return new WaitForSeconds(2f);

                if (_anims.Count > 0)
                {
                    foreach (var anim in _anims)
                    {
                        anim.ChangeToIdle();
                    }
                }
            }

        }
    }
}
