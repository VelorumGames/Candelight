using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Interactuables
{
    public class NodeInter : AInteractuables
    {
        private void OnEnable()
        {
            OnTrigger += Register;
        }

        public override void Interaction()
        {
            //GetComponent<NodeManager>().SetState(ENodeState.Incompleted);
            if (GetComponent<NodeManager>().GetNodeData().State != ENodeState.Completado)
            {
                WorldManager.Instance.SetCurrentNode(GetComponent<NodeManager>());
                WorldManager.Instance.LoadNode();
            }
        }

        void Register() => WorldManager.Instance.SetCurrentNode(GetComponent<NodeManager>());

        private void OnDisable()
        {
            OnTrigger -= Register;
        }
    }
}