using World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Map;

namespace Interactuables
{
    public class EndNodeInter : AInteractuables
    {
        public override void Interaction()
        {
            Debug.Log("Se completa el nodo");
            FindObjectOfType<SimpleRoomManager>().CurrentNodeInfo.Node.RegisterCompletedNode();
            SceneManager.LoadScene("WorldScene");
        }
    }
}