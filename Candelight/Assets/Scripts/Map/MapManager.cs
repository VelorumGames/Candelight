using Controls;
using Events;
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
                if (m_rooms >= MaxRooms) //Si se ha terminado la generacion de habitaciones
                {
                    if (OnRoomGenerationEnd != null) OnRoomGenerationEnd();
                }
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

        public event System.Action OnRoomGenerationEnd;


        private void Awake()
        {
            if (CurrentNodeInfo) _seed = CurrentNodeInfo.Seeds[CurrentNodeInfo.CurrentLevel];
            Random.InitState(_seed);

            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>().gameObject;

            if (CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Exploration)
            {
                switch (CurrentNodeInfo.Biome)
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
            }
            else MaxRooms = 5;

            if (MaxRooms > 0) RegisterNewRoom(-1, new Vector3(), ERoomSize.Medium);
        }

        private void OnEnable()
        {
            OnRoomGenerationEnd += RegisterRoomTypes;
            OnRoomGenerationEnd += EventCheck;
        }

        void EventCheck()
        {
            //Si el penultimo nivel es de exploracion, generamos el evento que corresponda
            if (CurrentNodeInfo.CurrentLevel == CurrentNodeInfo.Levels - 2 && CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Exploration && TryGetComponent<ExploreEventManager>(out var eventMan))
            {
                eventMan.GenerateEvent(this);
            }
            //Si es el ultimo nivel y es de calma, generamos el evento que corresponda
            else if (CurrentNodeInfo.CurrentLevel == CurrentNodeInfo.Levels - 1 && CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Calm && TryGetComponent<CalmEventManager>(out var calmEventMan))
            {
                calmEventMan.GenerateEvent(this);
            }
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
            //Debug.Log($"Se crea nueva habitacion ({CurrentRooms}) de tamano {size} conectada con habitacion {originalRoomID}");
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
            endRoom.RemoveEntities(); //Eliminamos los enemigos o npcs que pueda haber en la sala de salida
            GameObject torch = Instantiate(EndTorch, endRoom.transform);
            torch.transform.position = endRoom.GetRandomSpawnPoint().position;

            //Elegimos con cierta probabilidad que una de las habitaciones tenga una runa
            if (Random.value < RuneChance)
            {
                ARoom runeRoom = rooms[Random.Range(rooms.Count / 2, rooms.Count)];
                rooms.Remove(runeRoom);
                runeRoom.RoomType = ERoomType.Rune;
                runeRoom.IdText.text += " RUNE";

                runeRoom.ActivateRuneRoom();
            }
        }

        /// <summary>
        /// Finaliza el nivel
        /// </summary>
        public void EndLevel()
        {
            if (CurrentNodeInfo.CurrentLevel < CurrentNodeInfo.Levels - 1) //Si no es el ultimo nivel todavia
            {
                //Se apunta a la siguiente seed y se elije un tipo de nivel al que ir
                switch (CurrentNodeInfo.LevelTypes[++CurrentNodeInfo.CurrentLevel])
                {
                    case ELevel.Exploration:
                        SceneManager.LoadScene("LevelScene");
                        break;
                    case ELevel.Calm:
                        SceneManager.LoadScene("CalmScene");
                        break;
                    case ELevel.Challenge:
                        SceneManager.LoadScene("ChallengeScene");
                        break;
                }
            }
            else //Si es el ultimo nivel
            {
                //Se vuelve al mapa del mundo
                SceneManager.LoadScene("NodeEndScene");
            }
        }

        public GameObject GetRandomAvailableRoom(bool remove)
        {
            GameObject r = _rooms[Random.Range(0, _rooms.Count)];
            if (remove) _rooms.Remove(r);
            return r;
        }

        private void OnDisable()
        {
            OnRoomGenerationEnd -= RegisterRoomTypes;
            OnRoomGenerationEnd -= EventCheck;
        }
    }
}