using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace UI
{
    public class NodeBoxNotifier : MonoBehaviour
    {
        NodeInfoBox _box;
        NodeManager _node;

        private void Awake()
        {
            _box = FindObjectOfType<NodeInfoBox>();
            _node = GetComponent<NodeManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                NodeData data = _node.GetNodeData();

                string[] names = WorldManager.Instance.GetRandomNames(data.Biome);
                data.Name = names[0];
                data.Description = names[1];

                _box.RegisterNode(data.Name, data.Description, data.Biome, data.State.ToString());
                _box.ShowBox(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _box.ShowBox(false);
            }
        }
    }
}