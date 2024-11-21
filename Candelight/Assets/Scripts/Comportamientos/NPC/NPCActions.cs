using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Threading;

public class NPCActions : MonoBehaviour
{
    PlayerController player;
    NPCController controller;

    Vector3 target, currentPos;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        controller = GetComponent<NPCController>();
        currentPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
    }

    public bool isPlayerClose()
    {
        return false;
    }

    public void setRandomTarget()
    {
        Vector3 randomPos = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
        target = currentPos + randomPos;
        Debug.Log("Nuevo objetivo: " + target.x + ", " + target.z);
    }

    public void move()
    {
        controller.Move(target);
    }
}
