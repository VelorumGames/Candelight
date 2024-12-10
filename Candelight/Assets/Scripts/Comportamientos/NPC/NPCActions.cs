using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System.Threading;
using Unity.VisualScripting;
using BehaviourAPI.Core;

[RequireComponent(typeof(NPCController))]
public class NPCActions : MonoBehaviour
{
    PlayerController player;
    NPCController controller;
    protected Rigidbody rb;

    Vector3 target, currentPos;
    Vector2 direction;
    [SerializeField] protected float surpriseRange;
    bool surprised, arrived;
    public bool CanBeSurprised;
    float outTime;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        controller = GetComponent<NPCController>();
        rb = GetComponent<Rigidbody>();
        outTime = 0;
        arrived = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //currentPos = player.transform.position;
        target = currentPos;

        setRandomTarget();
        if (controller.CanMoveOnStart) move();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;


        if (CanBeSurprised && !surprised && Vector3.Distance(transform.position, player.transform.position) < surpriseRange)
        {
            surprised = true;
            //if (Random.value < 0.5f) 
            //{
            //    //llamar animación sorpresa
            //    StartCoroutine(controller.Surprise());
            //}
            StartCoroutine(controller.Surprise());
        }
    }

    public bool hasArrived()
    {
        if (arrived == true || currentPos == target)
        {
            arrived = false;
            return true;
        }
        return false;
    }

    public Status setRandomTarget()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
        target = currentPos + randomPos;
        //Debug.Log("Nuevo objetivo: " + target.x + ", " + target.z);

        return Status.Success;
    }

    public Status move()
    {
        if (controller.CanMoveOnStart)
        {
            //Debug.Log("Entro en rama mover");
            Vector3 direction;

            if (Vector3.Distance(transform.position, target) < 1.5f || outTime >= 5)
            {
                outTime = 0;
                arrived = true;
                return Status.Success;
            }

            direction = new Vector3((target - transform.position).x, 0f, (target - transform.position).z).normalized;

            if (!rb) rb = GetComponent<Rigidbody>();
            Vector3 force = Time.deltaTime * 100f * controller.getSpeed() * new Vector3(direction.x, 0f, direction.z);
            rb.AddForce(force, ForceMode.Force);

            outTime += Time.deltaTime;
            return Status.Running;
        }
        return Status.Success;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, surpriseRange);
    }
}
