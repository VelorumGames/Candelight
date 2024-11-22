using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class NodeStatusFeedback : MonoBehaviour
    {
        [SerializeField] GameObject[] _nodeLights;
        private void OnEnable()
        {
            if (GetComponentInParent<NodeManager>().GetNodeData().State == ENodeState.Completed)
            {
                foreach (var l in _nodeLights) l.SetActive(true);
            }
        }
    }
}