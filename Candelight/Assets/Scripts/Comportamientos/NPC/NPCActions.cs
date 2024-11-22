using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System.Threading;

public class NPCActions : MonoBehaviour
{
    PlayerController player;
    NPCController controller;

    Vector3 target, currentPos;
    Vector2 direction;

    int count;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        controller = GetComponent<NPCController>();
        //currentPos = player.transform.position;
        target = currentPos;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isPlayerClose()
    {
        return false;
    }

    public bool hasArrived()
    {
        if(currentPos == target || count >= 500)
        {
            return true;
        }
        count++;
        return false;
    }

    public void setRandomTarget()
    {
        Vector3 randomPos = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f));
        target = currentPos + randomPos;
        //Debug.Log("Nuevo objetivo: " + target.x + ", " + target.z);
    }

    public void move()
    {
        //Debug.Log("Entro en rama mover");
        StartCoroutine(controller.Move(target));
    }
}
