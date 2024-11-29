using Enemy;
using Interactuables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UI;
using static UnityEngine.Rendering.DebugUI;
using Player;

namespace Map
{
    public enum ERoomType
    {
        Start,
        Exit,
        Normal,
        Rune,
        Event
    }
    public abstract class ARoom : MonoBehaviour
    {
        protected int ID;
        public TextMeshPro IdText;
        public ERoomType RoomType = ERoomType.Normal;

        UIManager _uiMan;

        [SerializeField] Transform _runeTransform;
        [SerializeField] Transform[] _spawnPoints;
        public AnchorManager[] Anchors;

        protected PlayerController _cont;

        Vector2 _minimapOffset;

        public List<AnchorManager> AvailableAnchors = new List<AnchorManager>();
        public event System.Action OnPlayerEnter;
        public event System.Action OnPlayerExit;

        bool _hasEntered;

        protected void Awake()
        {
            _uiMan = FindObjectOfType<UIManager>();
            _cont = FindObjectOfType<PlayerController>();
        }

        protected void Start()
        {
            
        }

        public int GetID() => ID;
        public void SetID(int id)
        {
            ID = id;
            IdText.text = $"{ID}";
        }

        public void RemoveEntities()
        {
            EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
            foreach(var e in enemies) Destroy(e.gameObject);

            NpcInter[] npcs = GetComponentsInChildren<NpcInter>();
            foreach(var n in npcs) Destroy(n.gameObject);
        }

        public void ActivateRuneRoom() => _runeTransform.gameObject.SetActive(true);

        public Transform GetRandomSpawnPoint() => _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        public void SetMinimapOffset(Vector2 offset) => _minimapOffset = offset;
        public Vector2 GetMinimapOffset() => _minimapOffset;

        public ARoom[] GetAdyacentRooms()
        {
            List<ARoom> rooms = new List<ARoom>();
            if (Anchors.Length > 0)
            {
                foreach (var anchor in Anchors)
                {
                    rooms.Add(anchor.ConnectedAnchor.GetRoom());
                }
            }
            return rooms.ToArray();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_hasEntered)
            {
                _uiMan.ShowMinimapRoom(ID);
                OnPlayerTrigger();
                if (OnPlayerEnter != null) OnPlayerEnter();

                _hasEntered = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _hasEntered = false;
                if (OnPlayerExit != null) OnPlayerExit();
            }
        }

        protected abstract void OnPlayerTrigger();
    }
}