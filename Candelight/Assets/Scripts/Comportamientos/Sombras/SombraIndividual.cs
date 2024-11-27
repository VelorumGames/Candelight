
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos.Elementales;
using UnityEngine.UIElements;
using Enemy;
using Player;
using DG.Tweening;
using Animations;
using UI;

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
        /*[SerializeField]*/ GameObject _darkFireball;
        [SerializeField] float _projectileVelocity;
        private Rigidbody _rbMyDarkFireball;
        [SerializeField] private GameObject _myDarkFireball;
        [SerializeField] float _fireRate;
        private bool canShoot;

        [HideInInspector] public SombraComportamiento _scSombracomportamiento;
        private PlayerController _scPlayerController;
        public float dis;

        SombraAnimation _anim;

        private new void OnEnable()
        {
            base.OnEnable();
            OnDeath += IncreaseDeathCount;
        }

        private new void Awake()
        {
            base.Awake();
            _scPlayerController = FindObjectOfType<PlayerController>();
            _anim = GetComponent<SombraAnimation>();
        }
       


        private void IncreaseDeathCount(AController contr)
        {
            _scSombracomportamiento._sombrasdeaths++;
            
        }

        public void IncreaseSize()
        {
            
            if (transform.localScale.x <= _maxScale.x)
            {
                CurrentHP += _healthIncrease;
                transform.localScale = transform.localScale + new Vector3 (_scaleIncrease, _scaleIncrease, _scaleIncrease);

                FindObjectOfType<UIManager>().ShowTutorial("\"Las sombras absorbieron el poder del hechizo fantasmal\"", 4f);
            }
            
        }

        
        public void GoAway(float approachD)
        {
            
           Vector3 movimiento = new Vector3(transform.localPosition.x * approachD, transform.localPosition.y, transform.localPosition.z * approachD);

            
           transform.DOLocalMove(movimiento, 5f).Play().OnComplete(Shoot);
            
            dis = approachD;
        }

        public void Approach(float approachD)
        {
            
            Vector3 movimiento = new Vector3(transform.localPosition.x / approachD, transform.localPosition.y, transform.localPosition.z / approachD);
            transform.localPosition = movimiento;
            


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
                _scPlayerController.RecieveDamage(Info.BaseDamage);
                gameObject.SetActive(false);
                
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
                yield return new WaitForSeconds(Random.Range(0, 1.5f));

                _anim.ChangeToShoot();
                yield return new WaitForSeconds(1f);

                //_myDarkFireball = Instantiate(_darkFireball, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

                //Ahora con pool
                _myDarkFireball.SetActive(true);
                _myDarkFireball.transform.position = transform.position;

                _scProjectile = _myDarkFireball.GetComponent<Projectile>();
                _scProjectile.Damage = 2f;
                _projectileVelocity = 2f;
                _rbMyDarkFireball = _myDarkFireball.GetComponent<Rigidbody>();

                _rbMyDarkFireball.AddForce((_scPlayerController.transform.position - transform.position).normalized * 10f * _projectileVelocity, ForceMode.Impulse);

                yield return new WaitForSeconds(1f);

                _anim.ChangeToIdle();

                yield return new WaitForSeconds(_fireRate);

            }

            Vector3 origin = new Vector3(transform.localPosition.x / dis, transform.localPosition.y, transform.localPosition.z / dis);
            transform.DOLocalMove(origin, 5f).Play();

        }


        private new void OnDisable()
        {
            base.OnDisable();
            OnDeath -= IncreaseDeathCount;
        }

    }
}


