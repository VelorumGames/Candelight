using Animations;
using Hechizos;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    enum EMurcielagoState
    {
        Normal,
        Attack,
        Confused
    }

    public class MurcielagoGigante : EnemyController
    {
        [SerializeField] float _aggro;
        [SerializeField] float _minPlayerVel;
        [SerializeField] float _attackRange;

        Action _updateAction;

        Rigidbody _playerRb;
        MurcielagoAnimation _anim;

        private new void Awake()
        {
            base.Awake();
            _playerRb = Player.GetComponent<Rigidbody>();
            _anim = GetComponentInChildren<MurcielagoAnimation>();
        }

        private new void Start()
        {
            base.Start();
            ChangeState(EMurcielagoState.Normal);
        }

        void ChangeState(EMurcielagoState state)
        {
            //Debug.Log("[MUERCIELAGO] Entra en estado: " + state.ToString());

            _updateAction = null;

            switch (state)
            {
                case EMurcielagoState.Normal:
                    _updateAction = CheckPlayer;
                    NormalStart();
                    break;
                case EMurcielagoState.Attack:
                    AttackStart();
                    break;
                case EMurcielagoState.Confused:
                    ConfusedStart();
                    break;
            }
        }


        #region Normal
        void NormalStart()
        {
            _anim.ChangeToDown();
            CanMove = false;
        }

        void CheckPlayer()
        {
            //Debug.Log($"Distancia: {Vector3.Distance(transform.position, Player.transform.position)} < {_aggro} && Velocidad: {_playerRb.velocity.magnitude} && {_minPlayerVel}");
            if (Vector3.Distance(transform.position, Player.transform.position) < _aggro && _playerRb.velocity.magnitude > _minPlayerVel)
            {
                //Debug.Log("PHANTOM CHECK: " + PhantomCheck());
                if (PhantomCheck()) ChangeState(EMurcielagoState.Confused);
                else ChangeState(EMurcielagoState.Attack);
            }
        }

        bool PhantomCheck()
        {
            foreach (var element in ARune.MageManager.GetActiveElements()) if (element is PhantomRune) return true;
            return false;
        }
        #endregion

        #region Attack
        void AttackStart()
        {
            CanMove = true;

            StopAllCoroutines();
            StartCoroutine(ManageAttack());
        }

        IEnumerator ManageAttack()
        {
            Vector3 target;
            while (!PhantomCheck())
            {
                target = Player.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)); //Para que tampoco sea exacto (es ciego)
                //Debug.Log("Se movera hacia: " + target);
                yield return StartCoroutine(MoveTowards(target, 3f));
                //Debug.Log("Ha llegado");
                if (Vector3.Distance(transform.position, Player.transform.position) < _attackRange) //Si entra en rango de ataque
                {
                    OnAttack(); //Ataca y se espera un poco para que la animacion se reproduzca con normalidad (y para darle tiempo al jugador de escapar)
                    CanMove = false;
                    _anim.ChangeToAttack();
                    yield return new WaitForSeconds(1.5f);
                    CanMove = true;
                }
            }

            ChangeState(EMurcielagoState.Confused);
        }

        #endregion

        #region Confused
        void ConfusedStart()
        {
            StopAllCoroutines();
            StartCoroutine(ConfusedStare());
        }

        IEnumerator ConfusedStare()
        {
            CanMove = false;

            while (PhantomCheck())
            {
                //Elegir una direccion en la que mirar
                int direction = UnityEngine.Random.Range(0, 4);
                switch(direction)
                {
                    case 0:
                        _anim.ChangeToDown();
                        break;
                    case 1:
                        _anim.ChangeToUp();
                        break;
                    case 2:
                        _anim.ChangeToLeft();
                        break;
                    case 3:
                        _anim.ChangeToRight();
                        break;
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 2f));
            }

            CanMove = true;

            ChangeState(EMurcielagoState.Attack);
        }

        #endregion


        private void Update()
        {
            if (_updateAction != null) _updateAction();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _aggro);
        }
    }
}