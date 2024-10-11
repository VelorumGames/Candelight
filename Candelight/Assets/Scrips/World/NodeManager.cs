using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace World
{
    public enum EBiome
    {
        A, B, C
    }

    public class NodeManager : MonoBehaviour
    {
        Collider[] _closeNodes;
        List<GameObject> ConnectedNodes = new List<GameObject>();

        [SerializeField] int _numLevels; //Numero de niveles en cada nodo
        [SerializeField] int[] _seedExtra; //Para cada nivel se genera una seed extra (basada en la seed actual). Esta sera la seed en la que se base el nivel concreto para su generacion
        [SerializeField] EBiome _biome;

        [SerializeField] TextMeshPro _text;

        IEnumerator Start()
        {
            _numLevels = Random.Range(1, 5);
            _seedExtra = new int[_numLevels];
            for(int i = 0; i < _numLevels; i++)
            {
                _seedExtra[i] = Random.Range(0, 999999);
            }

            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
            ConnectNode();
        }

        void ConnectNode()
        {
            _closeNodes = Physics.OverlapSphere(transform.position, 5f);
            if (_closeNodes.Length == 1) Destroy(gameObject);

            int connection = 0;
            foreach (var n in _closeNodes)
            {
                //Si encontramos un nodo valido que no este conectado a este todavia
                if (n.CompareTag("Node") && !n.GetComponent<NodeManager>().ConnectedNodes.Contains(gameObject) && connection < 3)
                {
                    //Dibujamos la linea de conexion
                    SpawnLine(transform.position, n.transform.position);

                    //Registramos la conexion en ambos nodos
                    ConnectedNodes.Add(n.gameObject);
                    n.GetComponent<NodeManager>().ConnectedNodes.Add(gameObject);

                    connection++;
                }
            }
        }

        void SpawnLine(Vector3 start, Vector3 end)
        {
            GameObject lineGO = new GameObject("Connection");
            lineGO.transform.parent = transform;
            LineRenderer line = lineGO.AddComponent<LineRenderer>();

            Vector3[] positions = new Vector3[2];
            positions[0] = start;
            positions[1] = end;
            line.widthMultiplier = 0.01f;

            line.SetPositions(positions);
        }

        public void SetBiome(EBiome b)
        {
            _biome = b;
            _text.text = _biome.ToString();
        }

        //Debugging
        private void OnMouseDown()
        {
            WorldManager.Instance.SetCurrentNode(_numLevels, _seedExtra, _biome);
            WorldManager.Instance.LoadNode();
        }
    }
}
