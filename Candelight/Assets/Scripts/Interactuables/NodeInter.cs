using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Interactuables
{
    public class NodeInter : AInteractuables
    {
        public override void Interaction()
        {
            GetComponent<NodeManager>().SetState(ENodeState.Incompleted);
            WorldManager.Instance.SetCurrentNode(GetComponent<NodeManager>());
            WorldManager.Instance.LoadNode();
        }
    }
}