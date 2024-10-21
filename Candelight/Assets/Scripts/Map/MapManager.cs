using Controls;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        GameObject _player;

        public Camera ConnectionBakeCam;

        public int MaxRooms;
        [SerializeField] int m_rooms;
        public int CurrentRooms
        {
            get => m_rooms;
            set
            {
                m_rooms = value;
                if (m_rooms >= MaxRooms) RegisterRoomTypes();
                Camera.main.transform.parent = _player.transform;
                Camera.main.transform.localPosition = new Vector3(0, 2.5f, -4.6f);
                Camera.main.transform.localRotation = Quaternion.Euler(32f, 0f, 0f);
            }
        }

        public float RuneChance;

        public GameObject[] SmallRooms;
        public GameObject[] MediumRooms;
        public GameObject[] LargeRooms;
        public GameObject EndTorch;

        List<List<int>> _roomGraph = new List<List<int>>();
        public List<GameObject> _rooms = new List<GameObject>();

        //Generacion de la habitacion
        public float RoomSeparation;
        public float LargeThreshold;
        public float MediumThreshold;
        public float SmallThreshold;

        public float ConnectionWidth;

        [SerializeField] int _seed;

        public Material ConnectionMat;
        public float ConnectionCollidersOffset;

        public NodeInfo CurrentNodeInfo;

        private void Awake()
        {
            if (CurrentNodeInfo) _seed = CurrentNodeInfo.Seeds[CurrentNodeInfo.CurrentLevel];
            Random.InitState(_seed);

            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _player = FindObjectOfType<PlayerController>().gameObject;
        }

        private void Start()
        {
            switch(CurrentNodeInfo.Biome)
            {
                case EBiome.A:
                    MaxRooms = 10;
                    break;
                case EBiome.B:
                    MaxRooms = 20;
                    break;
                case EBiome.C:
                    MaxRooms = 30;
                    break;
            }
            if (MaxRooms > 0) RegisterNewRoom(-1, new Vector3(), ERoomSize.Medium);
        }

        /// <summary>
        /// Comprueba si se ha alcanzado el maximo de habitaciones que permite el nivel
        /// </summary>
        /// <returns></returns>
        public bool CanCreateRoom() => CurrentRooms < MaxRooms;

        /// <summary>
        /// Se crea una nueva habitacion y se registra en los datos del mapa
        /// </summary>
        /// <param name="originalRoomID"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Se escogen habitaciones al azar para concretar su tipo
        /// </summary>
        void RegisterRoomTypes()
        {
            //Tomamos el script de cada habitacion
            List<ARoom> rooms = new List<ARoom>();
            foreach (var r in _rooms) rooms.Add(r.GetComponent<ARoom>());

            //Elegimos una sala de entrada y otra de salida y las descartamos
            ARoom startRoom = rooms[Random.Range(0, rooms.Count / 2)];
            rooms.Remove(startRoom);
            startRoom.RoomType = ERoomType.Start;
            startRoom.IdText.text += " START";
            _player.transform.position = startRoom.transform.position + 1f * Vector3.up;

            ARoom endRoom = rooms[Random.Range(rooms.Count / 2, rooms.Count)];
            rooms.Remove(endRoom);
            endRoom.RoomType = ERoomType.Exit;
            endRoom.IdText.text += " EXIT";
            GameObject torch = Instantiate(EndTorch, endRoom.transform);

            //Elegimos con cierta probabilidad que una de las habitaciones tenga una runa
            if (Random.value < RuneChance)
            {
                ARoom runeRoom = rooms[Random.Range(rooms.Count / 2, rooms.Count)];
                rooms.Remove(runeRoom);
                runeRoom.RoomType = ERoomType.Rune;
                runeRoom.IdText.text += " RUNE";
            }
        }

        /// <summary>
        /// Finaliza el nivel
        /// </summary>
        public void EndLevel()
        {
            if (CurrentNodeInfo.CurrentLevel < CurrentNodeInfo.Levels - 1) //Si no es el ultimo nivel todavia
            {
                //Se apunta a la siguiente seed
                CurrentNodeInfo.CurrentLevel++;
                SceneManager.LoadScene("LevelScene");
            }
            else //Si es el ultimo nivel
            {
                //Se vuelve al mapa del mundo
                CurrentNodeInfo.Node.RegisterCompletedNode();
                SceneManager.LoadScene("WorldScene");
            }
        }
    }
}