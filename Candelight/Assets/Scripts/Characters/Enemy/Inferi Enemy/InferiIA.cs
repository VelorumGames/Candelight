using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Player;
using Hechizos;
using Hechizos.Elementales;
using Animations;

public class InferiIA : EnemyController
{
    static InferiIA m_lider;
    private InferiIA lider
    {
        get => m_lider;
        set
        {
            m_lider = value;
            gameObject.name = lider == this ? "Inferi lider" : "Inferi";
        }
    }
        //todos los inferis tienen la misma referencia del lider.

    private Transform objetivoJugador, objetivoInferi; 

    private bool auxiliar = true;
    private bool auxiliar2 = true;

    InferiAnimation _anim;

    private new void Awake()
    {
        base.Awake();

        _anim = GetComponentInChildren<InferiAnimation>();
    }

    private new void OnEnable() //se ejecuta cada vez que se activan
    {
        base.OnEnable();

        lider = this; //el último inferi que se active se queda como líder.
    }

    void Update()
    {
        if (ClosePlayerCheck() || lider == this) //pasa al estado de ATAQUE
        {
            if (auxiliar)
            {
                if (lider != this) Debug.Log("ATAQUE");
                //Ir hacia el jugador.
                objetivoJugador = Player.transform;
                StopAllCoroutines();
                StartCoroutine(MoveToTarget(objetivoJugador, true));
            }
        }
        else //continúa en el estado de MOVIMIENTO
        {
            if (lider != this) Debug.Log($"MOVIMIENTO: {InferiLeaderCheck()}");
            if (InferiLeaderCheck())
            {
                if (auxiliar)
                {
                    if (lider != this) Debug.Log("HACIA EL LIDER");
                    objetivoInferi = lider.transform;
                    StopAllCoroutines();
                    StartCoroutine(MoveToTarget(objetivoInferi, false));
                }
            }
            else if (auxiliar2)
            {
                StopAllCoroutines();
                StartCoroutine(GoToRandomPos()); //Si no ve un inferi, entonces escoge un objetivo aleatorio al que caminar.
            }
        }

        if (PlayerFireCheck()) Slow(0.75f, 10f);
    }

    bool CloseToTargetCheck() => Vector3.Distance(transform.position, objetivoJugador.position) < 2f;

    IEnumerator MoveToTarget(Transform objetivo, bool puedeAtacar)
    {
        auxiliar = false;
        yield return StartCoroutine(MoveTowards(objetivo.position, 3f)); //MoveTowards termina antes de que llegue exactamente al objetivo.
        //comprobar si está cerca el jugador.
        if (CloseToTargetCheck() && puedeAtacar)
        {
            _anim.ChangeToAttack();
            OnAttack();
        }

        auxiliar = true;
    }

    bool ClosePlayerCheck()
    {
        if (lider != this) Debug.Log("A player: " + Vector3.Distance(transform.position, Player.transform.position));
        return Vector3.Distance(transform.position, Player.transform.position) < 2f;
    }

    //Ha visto a otro inferi.
    bool InferiLeaderCheck()
    {
        if (lider != this) Debug.Log("A lider" + Vector3.Distance(transform.position, lider.transform.position));
        return Vector3.Distance(transform.position, lider.transform.position) < 5f && Vector3.Distance(transform.position, lider.transform.position) > 1f;
    }

    bool PlayerFireCheck()
    {
        if (ARune.ActiveElements != null)
        {
            foreach (var el in ARune.ActiveElements)
            {
                if (el is FireRune) return true;
            }
        }
        return false;
    }
    IEnumerator GoToRandomPos()
    {
        auxiliar2 = false;
        if (lider != this) Debug.Log($"ALEATORIO: {!InferiLeaderCheck()} && {!ClosePlayerCheck()}");
        while (!InferiLeaderCheck() && !ClosePlayerCheck())
        {
            Vector3 target = transform.position + new Vector3(Random.Range(-2f, 2f), transform.position.y, Random.Range(-2f, 2f));
            //Debug.Log("Se encuentra nuevo target: " + target);
            yield return StartCoroutine(MoveTowards(target, 5f));
        }
        auxiliar2 = true;
    }
}
