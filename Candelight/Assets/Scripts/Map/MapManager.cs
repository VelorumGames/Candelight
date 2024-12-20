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
using Music;

namespace Map
{
    
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        PlayerController _cont;
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
                if (_rooms.Count >= _maxRooms) //Si se ha terminado la generacion de habitaciones
                {
                    _available = false;
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
        bool _available = true;
        MusicManager _music;

        private void Awake()
        {
            if (CurrentNodeInfo) _seed = CurrentNodeInfo.Seeds[CurrentNodeInfo.CurrentLevel];
            Random.InitState(_seed);
            
            _music = FindObjectOfType<MusicManager>();
            _cont = FindObjectOfType<PlayerController>();

            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _uiMan = FindObjectOfType<UIManager>();
            //Debug. Debe estar activado
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
            GameSettings.LoadedWorld = true;

            _uiMan.FadeFromBlack(1f, 2f);

            if (CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Exploration)
            {
                switch (CurrentNodeInfo.Biome)
                {
                    case EBiome.Durnia:
                        _maxRooms = 6;
                        break;
                    case EBiome.Temeria:
                        _maxRooms = 12;
                        break;
                    case EBiome.Idria:
                        _maxRooms = 20;
                        break;
                }
            }
            else _maxRooms = 5;

            if (_maxRooms > 0) NotifyNewRoom(RegisterNewRoom(-1, new Vector3(), new Vector2(), ERoomSize.Medium), new Vector2());
        }

        private void OnEnable()
        {
            OnRoomGenerationEnd += RegisterRoomTypes;
            OnRoomGenerationEnd += EventCheck;

            OnCombatStart += _cont.RegisterCombat;
            OnCombatStart += _music.StartCombatMusic;
            OnCombatEnd += _music.ReturnToExploreMusic;
            OnCombatEnd += _cont.FinishCombat;
            OnCombatEnd += _cont.ResetLastSpellTimer;
            OnCombatEnd += _uiMan.WinCombat;

            
        }

        void EventCheck()
        {
            //Debug. Las condiciones deben estar sin comentar

            //Si el penultimo nivel es de exploracion, generamos el evento que corresponda
            if (CurrentNodeInfo.CurrentLevel == CurrentNodeInfo.Levels - 2 && CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Exploration && CurrentNodeInfo.EventID != -1 && TryGetComponent<ExploreEventManager>(out var eventMan))
            {
                eventMan.GenerateEvent();
            }
            //Si es el ultimo nivel y es de calma, generamos el evento que corresponda
            else if (CurrentNodeInfo.CurrentLevel == CurrentNodeInfo.Levels - 1 && CurrentNodeInfo.LevelTypes[CurrentNodeInfo.CurrentLevel] == ELevel.Calm && CurrentNodeInfo.EventID != -1 && TryGetComponent<CalmEventManager>(out var calmEventMan))
            {
                calmEventMan.GenerateEvent();
            }
        }

        /// <summary>
        /// Comprueba si se ha alcanzado el maximo de habitaciones que permite el nivel
        /// </summary>
        /// <returns></returns>
        public bool CanCreateRoom()
        {
            return _rooms.Count < _maxRooms && _available;
        }

        /// <summary>
        /// Se crea una nueva habitacion y se registra en los datos del mapa
        /// </summary>
        /// <param name="originalRoomID"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public GameObject RegisterNewRoom(int originalRoomID, Vector3 position, Vector2 minimapOffset, ERoomSize size)
        {
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
            room.gameObject.name = $"ROOM {_rooms.Count - 1}";
            _rooms[_rooms.Count - 1].GetComponent<ARoom>().SetID(_rooms.Count - 1);

            return _rooms[_rooms.Count - 1];
        }

        public void NotifyNewRoom(GameObject newRoom, Vector2 minimapOffset)
        {
            //Gestion del minimapa
            newRoom.GetComponent<ARoom>().SetMinimapOffset(minimapOffset);
            _uiMan.RegisterMinimapRoom(newRoom.GetComponent<ARoom>().GetID(), minimapOffset, newRoom.GetComponent<ARoom>().RoomType);

            _currentRooms++;
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

            int exit = 30;
            while (startRoom is EnemyRoom && exit > 0)
            {
                startRoom = rooms[Random.Range(0, rooms.Count / 2)];
                exit--;
            }

            rooms.Remove(startRoom);
            _rooms.Remove(startRoom.gameObject);
            startRoom.RoomType = ERoomType.Start;
            //if (startRoom.IdText) startRoom.IdText.text += " START";
            startRoom.gameObject.name = "START ROOM";
            _uiMan.UpdateMinimapRoom(startRoom.GetID(), ERoomType.Start);
            _cont.transform.position = startRoom.GetRandomSpawnPoint().position + 0.35f * Vector3.up;

            ARoom endRoom = rooms[Random.Range(rooms.Count / 2, rooms.Count)];
            rooms.Remove(endRoom);
            _rooms.Remove(endRoom.gameObject);
            endRoom.RoomType = ERoomType.Exit;
            //if (endRoom.IdText) endRoom.IdText.text += " EXIT";
            endRoom.gameObject.name = "EXIT ROOM";
            _uiMan.UpdateMinimapRoom(endRoom.GetID(), ERoomType.Exit);
            endRoom.RemoveEntities(); //Eliminamos los enemigos o npcs que pueda haber en la sala de salida
            GameObject torch = Instantiate(EndTorch, endRoom.transform);
            torch.transform.position = endRoom.GetRandomSpawnPoint().position;

            //Elegimos con cierta probabilidad que una de las habitaciones tenga una runa
            if (Random.value < RuneChance)
            {
                ARoom runeRoom = rooms[Random.Range(rooms.Count / 2, rooms.Count)];
                rooms.Remove(runeRoom);
                _rooms.Remove(runeRoom.gameObject);
                runeRoom.RoomType = ERoomType.Rune;
                //runeRoom.IdText.text += " RUNE";
                runeRoom.gameObject.name = "RUNE ROOM";
                _uiMan.UpdateMinimapRoom(runeRoom.GetID(), ERoomType.Rune);

                runeRoom.ActivateRuneRoom();
            }
        }

        /// <summary>
        /// Finaliza el nivel
        /// </summary>
        public void EndLevel()
        {
            FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
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

            OnCombatStart -= _cont.RegisterCombat;
            OnCombatStart -= _music.StartCombatMusic;
            OnCombatEnd -= _music.ReturnToExploreMusic;
            OnCombatEnd -= _cont.FinishCombat;
            OnCombatEnd -= _cont.ResetLastSpellTimer;
            OnCombatEnd -= _uiMan.WinCombat;
        }
    }
}