using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        public int MaxRooms;
        [SerializeField] int m_rooms;
        public int CurrentRooms
        {
            get => m_rooms;
            set
            {
                m_rooms = value;
                //if (m_rooms >= MaxRooms) CreateTransitions(_roomGraph);
            }
        }

        public GameObject[] SmallRooms;
        public GameObject[] MediumRooms;
        public GameObject[] LargeRooms;

        List<List<int>> _roomGraph = new List<List<int>>();
        public List<GameObject> _rooms = new List<GameObject>();

        //Room generation
        public float RoomSeparation;
        public float LargeThreshold;
        public float MediumThreshold;
        public float SmallThreshold;

        public int Seed;

        public Material ConnectionMat;
        public float ConnectionCollidersOffset;

        private void Awake()
        {
            Random.InitState(Seed);

            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            RegisterNewRoom(-1, new Vector3(), ERoomSize.Medium);
        }

        public bool CanCreateRoom() => CurrentRooms < MaxRooms;

        public GameObject RegisterNewRoom(int originalRoomID, Vector3 position, ERoomSize size)
        {
            Debug.Log($"Se crea nueva habitacion ({CurrentRooms}) de tamano {size} conectada con habitacion {originalRoomID}");
            _roomGraph.Add(new List<int>());

            if (originalRoomID != -1)
            {
                //Guardamos en la original una referencia a la nueva (tamano maximo del grafo) y viceversa
                _roomGraph[originalRoomID].Add(_roomGraph.Count - 1);
                _roomGraph[_roomGraph.Count - 1].Add(originalRoomID);
            }

            //Generamos el gameobject de la nueva habitacion
            switch (size)
            {
                case ERoomSize.Small:
                    _rooms.Add(Instantiate(SmallRooms[Random.Range(0, SmallRooms.Length)], position, new Quaternion()));
                    break;
                case ERoomSize.Medium:
                    _rooms.Add(Instantiate(MediumRooms[Random.Range(0, MediumRooms.Length)], position, new Quaternion()));
                    break;
                case ERoomSize.Large:
                    _rooms.Add(Instantiate(LargeRooms[Random.Range(0, LargeRooms.Length)], position, new Quaternion()));
                    break;
            }

            _rooms[_rooms.Count - 1].GetComponent<ARoom>().SetID(CurrentRooms);

            CurrentRooms++;

            return _rooms[_rooms.Count - 1];
        }

        void CreateTransitions(List<List<int>> graph)
        {
            for (int id = 0; id < graph.Count; id++)
            {
                for(int neighbour = 0; neighbour < graph[id].Count; neighbour++)
                {
                    int neighbourID = graph[id][neighbour];
                    GameObject roomA = _rooms[id];
                    GameObject roomB = _rooms[neighbourID];

                    graph[neighbourID].Remove(id);

                    SetLinePoints(roomA, roomB);
                }
            }
        }

        void SetLinePoints(GameObject origin, GameObject final)
        {
            Debug.Log($"Se conectan: {origin.transform.position} con {final.transform.position}");

            GameObject lineGO = new GameObject("Transicion");
            lineGO.transform.parent = origin.transform;
            LineRenderer line = lineGO.AddComponent<LineRenderer>();

            //origin.GetComponentInChildren<AnchorManager>()

            line.positionCount = 2;
            line.widthMultiplier = 0.25f;
            Vector3[] positions = new Vector3[2];
            positions[0] = origin.transform.position;
            positions[1] = final.transform.position;
            line.SetPositions(positions);
        }
    }
}