using Controls;
using Events;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;
using UI;
using Visual;

namespace Map
{
    
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        GameObject _player;
        UIManager _uiMan;

        [Header("===ROOM GENERATION===")]
        [Space(10)]
        [SerializeField] int _seed;
        [SerializeField] int _maxRooms;
        [SerializeField] int m_rooms;
        int _currentRooms
        {
            get => m_rooms;
            set
            {
                m_rooms = value;
                if (m_rooms >= _maxRooms) //Si se ha terminado la generacion de habitaciones
                {
                    if (OnRoomGenerationEnd != null) OnRoomGenerationEnd();
                }
            }
        }

        public float RuneChance;

        [Space(10)]
        public GameObject[] DurniaSmallRooms;
        public GameObject[] DurniaMediumRooms;
        public GameObject[] DurniaLargeRooms;
        public GameObject[] TemeriaSmallRooms;
        public GameObject[] TemeriaMediumRooms;
        public GameObject[] TemeriaLargeRooms;
        public GameObject[] IdriaSmallRooms;
        public GameObject[] IdriaMediumRooms;
        public GameObject[] IdriaLargeRooms;

        [HideInInspector] public GameObject[] SmallRooms;
        [HideInInspector] public GameObject[] MediumRooms;
        [HideInInspector] public GameObject[] LargeRooms;

        public GameObject EndTorch;

        List<List<int>> _roomGraph = new List<List<int>>();
        List<GameObject> _rooms = new List<GameObject>();

        [Space(10)]
        //Generacion de la habitacion
        public float RoomSeparation;
        public float LargeThreshold;
        public float MediumThreshold;
        public float SmallThreshold;
        public float AnchorCastOrigin;

        [Space(20)]
        [Header("===CONNECTION GENERATION===")]
        [Space(10)]
        public float ConnectionWidth;
        public float ConnectionCollidersOffset;
        [Space(10)]
        public Material ConnectionMaterial;
        public Camera ConnectionBakeCam;
        

        [Space(20)]
        [Header("===WORLD & NODE INFO===")]
        [Space(10)]
        public WorldInfo World;
        public NodeInfo CurrentNodeInfo;

        public event System.Action OnRoomGenerationEnd;
        public event System.Action OnCombatStart;
        public event System.Action OnCombatEnd;

        bool _inCombat;

        private void Awake()
        {
            if (CurrentNodeInfo) _seed = CurrentNodeInfo.Seeds[CurrentNodeInfo.CurrentLevel];
            Random.InitState(_seed);

            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _uiMan = FindObjectOfType<UIManager>();
            FindObjectOfType<InputManager>().LoadControls(EControlMap.Level);

            ConnectionMaterial = FindObjectOfType<LightingManager>().GetConnectionMaterial(CurrentNodeInfo.Biome);

            switch(CurrentNodeInfo.Biome)
            {
                case EBiome.Durnia:
                    SmallRooms = DurniaSmallRooms;
                    MediumRooms = DurniaMediumRooms;
                    LargeRooms = DurniaLargeRooms;
                    break;
                case EBiome.Temeria:
                    SmallRooms = TemeriaSmallRooms;
                    MediumRooms = TemeriaMediumRooms;
                    LargeRooms = TemeriaLargeRooms;
                    break;
                case EBiome.Idria:
                    SmallRooms = IdriaSmallRooms;
                    MediumRooms = IdriaMediumRooms;
                    LargeRooms = IdriaLargeRooms;
                    break;
            }
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>().gameObject;

            if (CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Exploration)
            {
                switch (CurrentNodeInfo.Biome)
                {
                    case EBiome.Durnia:
                        _maxRooms = 10;
                        break;
                    case EBiome.Temeria:
                        _maxRooms = 20;
                        break;
                    case EBiome.Idria:
                        _maxRooms = 30;
                        break;
                }
            }
            else _maxRooms = 5;

            if (_maxRooms > 0) RegisterNewRoom(-1, new Vector3(), new Vector2(), ERoomSize.Medium);
        }

        private void OnEnable()
        {
            OnRoomGenerationEnd += RegisterRoomTypes;
            OnRoomGenerationEnd += EventCheck;
        }

        void EventCheck()
        {
            //Si el penultimo nivel es de exploracion, generamos el evento que corresponda
            //DEBUG
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
        public bool CanCreateRoom() => _currentRooms < _maxRooms;

        /// <summary>
        /// Se crea una nueva habitacion y se registra en los datos del mapa
        /// </summary>
        /// <param name="originalRoomID"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public GameObject RegisterNewRoom(int originalRoomID, Vector3 position, Vector2 minimapOffset, ERoomSize size)
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
            GameObject room = null;
            switch (size)
            {
                case ERoomSize.Small:
                    room = Instantiate(SmallRooms[Random.Range(0, SmallRooms.Length)], position, new Quaternion());
                    break;
                case ERoomSize.Medium:
                    room = Instantiate(MediumRooms[Random.Range(0, MediumRooms.Length)], position, new Quaternion());
                    break;
                case ERoomSize.Large:
                    room = Instantiate(LargeRooms[Random.Range(0, LargeRooms.Length)], position, new Quaternion());
                    break;
                default:
                    room = Instantiate(SmallRooms[Random.Range(0, SmallRooms.Length)], position, new Quaternion());
                    break;
            }
            _rooms.Add(room);
            room.gameObject.name = "ROOM " + _currentRooms;
            _rooms[_rooms.Count - 1].GetComponent<ARoom>().SetID(_currentRooms);

            //Gestion del minimapa
            room.GetComponent<ARoom>().SetMinimapOffset(minimapOffset);
            _uiMan.RegisterMinimapRoom(_currentRooms, minimapOffset, room.GetComponent<ARoom>().RoomType);

            _currentRooms++;
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
            _uiMan.UpdateMinimapRoom(startRoom.GetID(), ERoomType.Start);
            _player.transform.position = startRoom.transform.position + 1f * Vector3.up;

            ARoom endRoom = rooms[Random.Range(rooms.Count / 2, rooms.Count)];
            rooms.Remove(endRoom);
            endRoom.RoomType = ERoomType.Exit;
            endRoom.IdText.text += " EXIT";
            _uiMan.UpdateMinimapRoom(endRoom.GetID(), ERoomType.Exit);
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
                _uiMan.UpdateMinimapRoom(runeRoom.GetID(), ERoomType.Rune);

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

        public void StartCombat()
        {
            if (!_inCombat)
            {
                Debug.Log("EMPIEZA COMBATE");
                if (OnCombatStart != null) OnCombatStart();

                _inCombat = true;
            }
        }
        public void EndCombat()
        {
            if (_inCombat)
            {
                Debug.Log("TERMINA COMBATE");
                if (OnCombatEnd != null) OnCombatEnd();

                _inCombat = false;
            }
        }

        private void OnDisable()
        {
            OnRoomGenerationEnd -= RegisterRoomTypes;
            OnRoomGenerationEnd -= EventCheck;
        }
    }
}