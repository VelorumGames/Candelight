using Player;
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

        WorldManager _worldMan;

        private void Awake()
        {
            _node = GetComponent<NodeManager>();
            _worldMan = FindObjectOfType<WorldManager>();
        }

        private void OnEnable()
        {
            _box = FindObjectOfType<NodeInfoBox>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("TRIGGER: " + other.gameObject.name);
            if (other.CompareTag("Player"))
            {
                Invoke("StopPlayer", 0.5f);
                
            }
        }

        public void StopPlayer()
        {
            FindObjectOfType<PlayerController>().GetComponent<Rigidbody>().useGravity = false;
            FindObjectOfType<PlayerController>().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            StartCoroutine(ShowData());
        }

        IEnumerator ShowData()
        {
            //yield return new WaitUntil(() => _worldMan.Loaded);

            NodeData data = _node.GetNodeData();
            while (data.Name == "" || data.Name == " ")
            {
                //Debug.Log($"COMPROBANDO ({_node.gameObject.name}): {_node.GetNodeData().Name}");
                data = _node.GetNodeData();
                yield return null;
            }
            //Debug.Log($"COMPR_TRUE ({_node.gameObject.name}): {_node.GetNodeData().Name}");
            _box.RegisterNode(_node, data.Name, data.Description, data.Biome, data.State.ToString(), data.LevelTypes);
            _box.ShowBox(true);
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