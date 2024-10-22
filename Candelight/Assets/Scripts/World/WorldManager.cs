using Controls;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;

        public List<List<int>> WorldMap = new List<List<int>>();
        List<GameObject> _nodes = new List<GameObject>();
        public int NumNodes;
        GameObject _player;

        public int MaxNodes;
        public GameObject NodePrefab;
        [SerializeField] Vector2 _spawnRange;
        [SerializeField] float _minDistBetweenNodes;
        [SerializeField] Transform _worldParent;

        public Material biomeA;
        public Material biomeB;
        public Material biomeC;

        [SerializeField] float _biomeOffset;
        [SerializeField] float _biomeSize;
        [SerializeField] float _biomeAThreshold;
        [SerializeField] float _biomeBThreshold;

        public WorldInfo World;
        public NodeInfo CurrentNodeInfo;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _player = FindObjectOfType<PlayerController>().gameObject;

            //Si es la primera vez que se visita esta escena
            if (!World.World)
            {
                World.CompletedNodes = 0;
                DontDestroyOnLoad(_worldParent.gameObject);
            }

            //Se fija la semilla
            Random.InitState(World.Seed);

            for (int id = 0; id < MaxNodes; id++)
            {
                WorldMap.Add(new List<int>());
            }
        }

        private void Start()
        {
            CurrentNodeInfo.CurrentLevel = 0;

            if (!World.World) //Si no se ha generado mundo previamente
            {
                SpawnRandomNodes();

                _biomeOffset = Random.Range(0f, 100f);
                GenerateBiomes();
                World.World = _worldParent.gameObject;

            }
            else
            {
                Debug.Log("INFO: El mundo ya se ha generado, por lo que no se vuelve a generar");
                World.World.SetActive(true);
            }
            try
            {
                MovePlayerToLastNode(CurrentNodeInfo.Node.transform);
            }
            catch (System.Exception e)
            {
                Debug.Log("INFO: Se ha detectado datos de un mundo previo inaccesibles. Se genera un nuevo mundo: " + e);
                World.World = null;
                Start();
            }
        }

        /// <summary>
        /// Genera un numero de nodos aleatorios en una region, asegurando una distancia minima entre ellos
        /// </summary>
        void SpawnRandomNodes()
        {
            for (int id = 0; id < MaxNodes; id++)
            {
                //Genero un nodo y lo coloco en una posicion aleatoria
                GameObject node = Instantiate(NodePrefab, _worldParent);
                node.transform.position = new Vector3(Random.Range(-_spawnRange.x, _spawnRange.x), 0, Random.Range(-_spawnRange.y, _spawnRange.y));

                //Check de distancia minima
                int secure = 0;
                while(_nodes.Count > 0 && ClosestNodeCheck(node.transform))
                {
                    node.transform.position = new Vector3(Random.Range(-_spawnRange.x, _spawnRange.x), 0, Random.Range(-_spawnRange.y, _spawnRange.y));
                    if (secure++ == 50) break;
                }

                _nodes.Add(node);
            }
        }

        private void Update()
        {
            //GenerateBiomes();
        }

        /// <summary>
        /// Se generan los biomas a raiz de ruido perlin
        /// </summary>
        void GenerateBiomes()
        {
            foreach (var node in _nodes)
            {
                float maxDataRange = 20f;
                float biomeData = maxDataRange * Mathf.PerlinNoise(_biomeOffset + node.transform.position.x / _biomeSize, _biomeOffset + node.transform.position.z / _biomeSize);
                
                EBiome biome = biomeData < maxDataRange * _biomeAThreshold ? EBiome.A : biomeData < maxDataRange * _biomeBThreshold ? EBiome.B : EBiome.C;
                node.GetComponent<NodeManager>().SetBiome(biome);
                switch (biome)
                {
                    case EBiome.A:
                        node.GetComponent<Renderer>().material = biomeA;
                        break;
                    case EBiome.B:
                        node.GetComponent<Renderer>().material = biomeB;
                        break;
                    case EBiome.C:
                        node.GetComponent<Renderer>().material = biomeC;
                        break;
                }
            }

            GenerateStart();
        }

        void GenerateStart()
        {
            foreach (var node in _nodes)
            {
                if (node.TryGetComponent<NodeManager>(out var nodeMan) && nodeMan.StartNodeCheck())
                {
                    GameObject _startNode = node;
                    nodeMan.Text.text += " START";
                    nodeMan.SetState(ENodeState.Explored);
                    CurrentNodeInfo.Node = nodeMan; //Marcamos este como nodo inicial
                    return;
                }
            }

            Debug.LogWarning("ERROR: No se ha encontrado ningun nodo de entrada valido");
        }

        void MovePlayerToLastNode(Transform node)
        {
            _player.transform.position = new Vector3(node.position.x, _player.transform.position.y, node.position.z);
        }

        /// <summary>
        /// Comprobacion de la distancia de un nodo al nodo mas cercano
        /// </summary>
        /// <param name="nT"></param>
        /// <returns></returns>
        bool ClosestNodeCheck(Transform nT)
        {
            float minDist = 9999;
            foreach (var n in _nodes)
            {
                float dist = Vector3.Distance(nT.position, n.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }
            if (minDist < 9999) return minDist < _minDistBetweenNodes;
            else return false;
        }

        /// <summary>
        /// Se registra un nodo como nodo actual
        /// </summary>
        /// <param name="levels"></param>
        /// <param name="seeds"></param>
        /// <param name="biome"></param>
        public void SetCurrentNode(NodeManager node)
        {
            NodeData data = node.GetNodeData();
            CurrentNodeInfo.Levels = data.NumLevels;
            CurrentNodeInfo.Seeds = data.SeedExtra;
            CurrentNodeInfo.Biome = data.Biome;
            CurrentNodeInfo.Node = node;
        }

        /// <summary>
        /// Se carga la escena del nodo
        /// </summary>
        public void LoadNode()
        {
            _worldParent.gameObject.SetActive(false);
            SceneManager.LoadScene("LevelScene");
        }
    }
}
