
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos.Elementales;
using UnityEngine.UIElements;
using Enemy;
using Player;

namespace Comportamientos.Sombra
{

    public class SombraIndividual : EnemyController
    {
        //Variables:

        [SerializeField] Vector3 _maxScale;
        [SerializeField] float _healthIncrease;
        [SerializeField] float _scaleIncrease;
        [SerializeField] float _approachDistance;

        private Projectile _scProjectile;
        private GameObject _darkFireball;
        [SerializeField] float _projectileVelocity;
        private Rigidbody _rbMyDarkFireball;
        private GameObject _myDarkFireball;
        [SerializeField] float _fireRate;
        private bool canShoot;

        private SombraComportamiento _scSombracomportamiento;
        private EnemyController _scEnemyController;
        private PlayerController _scPlayerController;
        private EnemyInfo _soSombraInfo;
        private float _bodyDamage;


        private void Awake()
        {
            _bodyDamage = _soSombraInfo.BaseDamage;
        }


        public void IncreaseSize()
        {
            if (transform.localScale.x <= _maxScale.x)
            {
                CurrentHP += _healthIncrease;
                CurrentHP += _scaleIncrease;
            }
        }

        public void Approach(float approachD)
        {
            transform.localPosition = new Vector3(transform.localPosition.x * approachD, transform.localPosition.y * approachD, transform.localPosition.z);
        }

        public void Shoot()
        {
            StartCoroutine(ManageFireRate());
        }


        private void OnTriggerEnter(Collider other)
        {

            //Comprueba si le ha tocado un ataque fantasmal

            if (other.TryGetComponent(out ASpell ASpellScript))
            {
                foreach (AElementalRune spell in ASpellScript.Elements)
                {
                    if (spell is PhantomRune)
                    {
                        ASpellScript.gameObject.SetActive(false);
                        IncreaseSize();
                    }
                }
            }

            // Si hace contacto con el jugador 
            if (other.TryGetComponent(out PlayerController _scPlayerController))
            {
                gameObject.SetActive(false);
                _scPlayerController.RecieveDamage(_bodyDamage);
            }

            /*
            ASpell scriptASpell = other.GetComponent<ASpell>();

            if (scriptASpell != null)
            {

            }
            */

        }

        IEnumerator ManageFireRate()
        {
            while (_scSombracomportamiento.EquipadoFuego())
            {


                _myDarkFireball = Instantiate(_darkFireball, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                _scProjectile.Damage = 2f;
                _projectileVelocity = 1f;
                _rbMyDarkFireball = _myDarkFireball.GetComponent<Rigidbody>();

                _rbMyDarkFireball.AddForce(_myDarkFireball.transform.forward * _projectileVelocity);
                yield return new WaitForSeconds(_fireRate);

            }
        }

    }
}


