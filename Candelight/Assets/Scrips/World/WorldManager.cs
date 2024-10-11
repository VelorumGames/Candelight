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

        public int MaxNodes;
        public GameObject NodePrefab;
        [SerializeField] Vector2 _spawnRange;
        [SerializeField] float _minDistBetweenNodes;
        [SerializeField] Transform _worldParent;

        public Material biomeA;
        public Material biomeB;
        public Material biomeC;

        [SerializeField] float _biomeOffset;
        [SerializeField] float _biomeAThreshold;
        [SerializeField] float _biomeBThreshold;

        public WorldInfo World;
        public NodeInfo CurrentNodeInfo;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            //Se fija la semilla
            Random.InitState(World.Seed);

            for (int id = 0; id < MaxNodes; id++)
            {
                WorldMap.Add(new List<int>());
            }
        }

        private void Start()
        {
            SpawnRandomNodes();

            _biomeOffset = Random.Range(0f, 100f);
            GenerateBiomes();
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
            GenerateBiomes();
        }

        /// <summary>
        /// Se generan los biomas a raiz de ruido perlin
        /// </summary>
        void GenerateBiomes()
        {
            foreach (var node in _nodes)
            {
                float maxDataRange = 20f;
                float biomeData = maxDataRange * Mathf.PerlinNoise(_biomeOffset + node.transform.position.x / 20, _biomeOffset + node.transform.position.z / 20);
                
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
        public void SetCurrentNode(int levels, int[] seeds, EBiome biome)
        {
            CurrentNodeInfo.Levels = levels;
            CurrentNodeInfo.Seeds = seeds;
            CurrentNodeInfo.Biome = biome;
        }

        /// <summary>
        /// Se carga la escena del nodo
        /// </summary>
        public void LoadNode()
        {
            SceneManager.LoadScene("LevelScene");
        }
    }
}
