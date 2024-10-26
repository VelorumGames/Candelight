using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace World
{
    public enum EBiome
    {
        Undefined, A, B, C
    }
    public enum ELevel
    {
        Exploration,
        Calm,
        Challenge
    }
    public enum ENodeState
    {
        Undiscovered,
        Explored,
        Incompleted,
        Completed
    }
    public enum EEventSolution
    {
        None,
        Ignored,
        Failed,
        AltCompleted,
        Completed
    }

    public struct NodeData
    {
        public int NumLevels; //Numero de niveles en cada nodo
        public int[] SeedExtra; //Para cada nivel se genera una seed extra (basada en la seed actual). Esta sera la seed en la que se base el nivel concreto para su generacion
        public ELevel[] LevelTypes;
        public EBiome Biome;
        public ENodeState State;
        public int EventID;
        public EEventSolution EventSolution;
    }

    public class NodeManager : MonoBehaviour
    {
        Collider[] _closeNodes;
        public List<GameObject> ConnectedNodes = new List<GameObject>();
        [SerializeField] float _connectRadius;

        NodeData _data;
        public Material UnexploredLineMat;
        public Material ExploredLineMat;

        public TextMeshPro Text;
        public GameObject Fog;

        bool _completedConnection;

        private void Awake()
        {
            _data.Biome = EBiome.Undefined;
        }

        IEnumerator Start()
        {
            gameObject.name = $"nodo {WorldManager.Instance.NumNodes++}";

            _data.NumLevels = Random.Range(1, 5);
            _data.SeedExtra = new int[_data.NumLevels];
            _data.LevelTypes = new ELevel[_data.NumLevels];
            for(int i = 0; i < _data.NumLevels; i++)
            {
                _data.SeedExtra[i] = Random.Range(0, 999999);
                _data.LevelTypes[i] = (ELevel) Random.Range(0, 3);
            }
            _data.LevelTypes[0] = 0; //El primer nivel siempre sera de exploracion

            if (EventCheck()) _data.EventID = Random.Range(0, 5);
            _data.EventSolution = EEventSolution.None;

            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
            ConnectNode();
        }

        void ConnectNode()
        {
            _closeNodes = Physics.OverlapSphere(transform.position, _connectRadius);
            if (_closeNodes.Length == 1) Destroy(gameObject);

            int connection = 0;
                       
            foreach (var n in _closeNodes)
            {
                //Si encontramos un nodo valido que no este conectado a este todavia
                if (n.CompareTag("Node") && !ConnectedNodes.Contains(n.gameObject) && !n.GetComponent<NodeManager>().ConnectedNodes.Contains(gameObject) && connection < 4 && n.GetComponent<NodeManager>().ConnectedNodes.Count < 4)
                {
                    //Registramos la conexion en ambos nodos
                    ConnectedNodes.Add(n.gameObject);
                    n.GetComponent<NodeManager>().ConnectedNodes.Add(gameObject);

                    connection++;
                }
            }
            _completedConnection = true;
        }

        void SpawnLine(Vector3 start, Vector3 end, Material lineMat)
        {
            GameObject lineGO = new GameObject("Connection");
            lineGO.transform.parent = transform;
            LineRenderer line = lineGO.AddComponent<LineRenderer>();

            Vector3[] positions = new Vector3[2];
            positions[0] = start;
            positions[1] = end;
            line.widthMultiplier = 0.1f;
            line.material = lineMat;

            line.SetPositions(positions);
        }

        public void SetBiome(EBiome b)
        {
            _data.Biome = b;
            Text.text = _data.Biome.ToString();
        }

        public void SetState(ENodeState s)
        {
            Debug.Log($"Se registra nodo {gameObject.name} como: {s}");
            _data.State = s;
            Text.text += _data.State.ToString(); //Simplemente una guia para saber si se registra bien el estado
            if (s == ENodeState.Explored)
            {
                Debug.Log("Connected nodes: " + ConnectedNodes.Count);
                if (_completedConnection)
                {
                    foreach (var n in ConnectedNodes)
                    {
                        //Dibujamos la linea de conexion
                        SpawnLine(transform.position, n.transform.position, UnexploredLineMat);
                        Fog.SetActive(false);
                    }
                }
                else
                {
                    StartCoroutine(DelayedConnection());
                }
            }
            else if (s == ENodeState.Completed)
            {
                foreach (var n in ConnectedNodes)
                {
                    //Dibujamos la linea de conexion
                    if (n.GetComponent<NodeManager>().GetNodeData().State == ENodeState.Completed) SpawnLine(transform.position, n.transform.position, ExploredLineMat);
                    Fog.SetActive(false);
                }
            }
        }

        IEnumerator DelayedConnection()
        {
            Debug.Log("Se espera hasta que se complete el proceso de conexion");

            yield return new WaitUntil(() => _completedConnection);

            Debug.Log("Nuevo numero de nodos conectados: " + ConnectedNodes.Count);
            foreach (var n in ConnectedNodes)
            {
                //Dibujamos la linea de conexion
                SpawnLine(transform.position, n.transform.position, UnexploredLineMat);
                Fog.SetActive(false);
            }
            yield return null;
        }

        /// <summary>
        /// Comprobamos si se trata de un nodo de bioma facil rodeado de nodos de bioma facil
        /// </summary>
        /// <returns></returns>
        public bool StartNodeCheck()
        {
            bool start = true;

            if (_data.Biome != EBiome.A) start = false;
            foreach(var n in ConnectedNodes)
            {
                Debug.Log(n.GetComponent<NodeManager>().GetNodeData().Biome);
                if (n.GetComponent<NodeManager>().GetNodeData().Biome != EBiome.A)
                {
                    start = false;
                    break;
                }
            }

            return start;
        }

        public NodeData GetNodeData() => _data;

        public void RegisterCompletedNode()
        {
            SetState(ENodeState.Completed);
            WorldManager.Instance.World.CompletedNodes++;
            foreach(var node in ConnectedNodes)
            {
                node.GetComponent<NodeManager>().SetState(ENodeState.Explored);
            }
        }

        bool EventCheck() => _data.NumLevels > 1 && _data.LevelTypes[_data.LevelTypes.Length - 1] == ELevel.Calm && _data.LevelTypes[_data.LevelTypes.Length - 2] == ELevel.Exploration;
    }
}
