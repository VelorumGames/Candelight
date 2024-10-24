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

        public int GetID() => ID;
        public void SetID(int id)
        {
            ID = id;
            IdText.text = $"{ID}";
        }

        public void ActivateRuneRoom() => _runeTransform.gameObject.SetActive(true);
    }
}