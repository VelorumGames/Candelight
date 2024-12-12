using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace World
{
    public enum EBiome
    {
        Undefined, Durnia, Temeria, Idria
    }
    public enum ELevel
    {
        Exploration,
        Calm,
        Challenge
    }
    public enum ENodeState
    {
        Desconocido,
        Inexplorado,
        Completado
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
        public string Name;
        public string Description;
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
        public int Id;

        Collider[] _closeNodes;
        public List<GameObject> ConnectedNodes = new List<GameObject>();
        public Dictionary<NodeManager, GameObject> Connections = new Dictionary<NodeManager, GameObject>();
        [SerializeField] float _connectRadius;

        NodeData _data;
        public Material UnexploredLineMat;
        public Material ExploredLineMat;

        public TextMeshPro Text;
        public GameObject Fog;

        int _maxConNodes = 3;

        [SerializeField] GameObject[] _biomeGOs;

        private void Awake()
        {
            _data.Biome = EBiome.Undefined;
        }

        IEnumerator Start()
        {
            gameObject.name = $"nodo {WorldManager.Instance.NumNodes++}";

            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
            ConnectNode();

            _data.NumLevels = Random.Range(1, 5);
            _data.SeedExtra = new int[_data.NumLevels];
            _data.LevelTypes = new ELevel[_data.NumLevels];
            for(int i = 0; i < _data.NumLevels; i++)
            {
                _data.SeedExtra[i] = Random.Range(0, 999999);
                _data.LevelTypes[i] = (ELevel) Random.Range(0, 3);
            }
            _data.LevelTypes[0] = 0; //El primer nivel siempre sera de exploracion

            string[] names = WorldManager.Instance.GetRandomNames(_data.Biome);
            _data.Name = names[0];
            _data.Description = names[1];

            _data.EventID = -1;

            if (EventCheck())
            {
                switch(_data.Biome)
                {
                    case EBiome.Durnia:
                        _data.EventID = Random.Range(0, 4);
                        //Debug.Log($"({Id}) HE GENERADO EL EVENTO EN DURNIA: " + _data.EventID);
                        break;
                    case EBiome.Temeria:
                        _data.EventID = Random.Range(0, 4);
                        //Debug.Log($"({Id}) HE GENERADO EL EVENTO EN TEMERIA: " + _data.EventID);
                        break;
                    case EBiome.Idria:
                        _data.EventID = Random.Range(0, 2);
                        //Debug.Log($"({Id}) HE GENERADO EL EVENTO EN IDRIA: " + _data.EventID);
                        break;
                }
            }
            _data.EventSolution = EEventSolution.None;

            CheckForPreviousGameNode();
        }

        void CheckForPreviousGameNode()
        {
            if (WorldManager.Instance.World.LoadedPreviousGame)
            {
                foreach (var i in WorldManager.Instance.World.CompletedIds)
                {
                    if (Id == i)
                    {
                        RegisterCompletedNode();
                        GetComponentInChildren<NodeStatusFeedback>().RegisterNodeLights();
                    }
                }
            }
        }

        void ConnectNode()
        {
            _closeNodes = Physics.OverlapSphere(transform.position, _connectRadius);
            if (_closeNodes.Length == 1) Destroy(gameObject);

            int connection = 0;
                       
            foreach (var n in _closeNodes)
            {
                //Si encontramos un nodo valido que no este conectado a este todavia
                if (n.CompareTag("Node") && !ConnectedNodes.Contains(n.gameObject) && !n.GetComponent<NodeManager>().ConnectedNodes.Contains(gameObject) && connection < _maxConNodes && n.GetComponent<NodeManager>().ConnectedNodes.Count < _maxConNodes)
                {
                    //Registramos la conexion en ambos nodos
                    ConnectedNodes.Add(n.gameObject);
                    n.GetComponent<NodeManager>().ConnectedNodes.Add(gameObject);

                    connection++;
                }
            }
        }

        void SpawnLine(NodeManager startNode, NodeManager endNode, Material lineMat)
        {
            Vector3 start = startNode.transform.position;
            Vector3 end = endNode.transform.position;

            GameObject lineGO = new GameObject("Connection");
            lineGO.transform.parent = transform;
            LineRenderer line = lineGO.AddComponent<LineRenderer>();

            Vector3[] positions = new Vector3[2];
            positions[0] = start;
            positions[1] = end;
            line.widthMultiplier = 0.1f;
            line.material = lineMat;

            line.SetPositions(positions);

            Connections.Add(endNode, lineGO);
        }

        public void SetBiome(EBiome b)
        {
            _data.Biome = b;
            Text.text = _data.Biome.ToString();

            for (int i = 0; i < _biomeGOs.Length; i++)
            {
                if (i == (int) b - 1)
                {
                    _biomeGOs[i].SetActive(true);
                }
            }
        }

        public void SetState(ENodeState s)
        {
            _data.State = s;
            Text.text += _data.State.ToString(); //Simplemente una guia para saber si se registra bien el estado

            if (s == ENodeState.Completado)
            {
                foreach (var n in ConnectedNodes)
                {
                    if (n.gameObject != gameObject)
                    {
                        NodeManager targetNode = n.GetComponent<NodeManager>();

                        //Comprobamos si la linea no ha sido creada (registrada) antes. En ese caso, la sobreescribimos
                        if (targetNode.Connections.ContainsKey(this))
                        {
                            targetNode.CleanLine(this);
                            SpawnLine(this, targetNode, ExploredLineMat);
                        }
                        else SpawnLine(this, targetNode, UnexploredLineMat);

                        Fog.SetActive(false);
                    }
                }
            }
        }

        public void CleanLine(NodeManager targetNode)
        {
            Debug.Log("Se destruira: " + Connections[targetNode].name);
            Destroy(Connections[targetNode]);
            Connections.Remove(targetNode);
        }

        /// <summary>
        /// Comprobamos si se trata de un nodo de bioma facil rodeado de nodos de bioma facil
        /// </summary>
        /// <returns></returns>
        public bool StartNodeCheck()
        {
            bool start = true;

            if (_data.Biome != EBiome.Durnia) start = false;
            foreach(var n in ConnectedNodes)
            {
                if (n.GetComponent<NodeManager>().GetNodeData().Biome != EBiome.Durnia)
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
            Debug.Log("Se completa el nodo");

            SetState(ENodeState.Completado);

            if (!WorldManager.Instance.World.LoadedPreviousGame) WorldManager.Instance.World.CompletedIds.Add(Id);
            WorldManager.Instance.World.CompletedNodes++;

            NodeManager nodeMan;
            foreach (var node in ConnectedNodes)
            {
                nodeMan = node.GetComponent<NodeManager>();
                if (gameObject != node.gameObject && nodeMan.GetNodeData().State != ENodeState.Completado) nodeMan.SetState(ENodeState.Inexplorado);
            }
        }

        bool EventCheck() => _data.NumLevels > 1 && _data.LevelTypes[_data.LevelTypes.Length - 1] == ELevel.Calm && _data.LevelTypes[_data.LevelTypes.Length - 2] == ELevel.Exploration;
    }
}
