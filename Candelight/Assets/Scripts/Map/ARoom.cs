using Enemy;
using Interactuables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        [SerializeField] Transform _runeTransform;
        [SerializeField] Transform[] _spawnPoints;

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
    }
}