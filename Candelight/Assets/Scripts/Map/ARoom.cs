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
        Rune
    }
    public abstract class ARoom : MonoBehaviour
    {
        protected int ID;
        public TextMeshPro IdText;
        public ERoomType RoomType = ERoomType.Normal;

        public int GetID() => ID;
        public void SetID(int id)
        {
            ID = id;
            IdText.text = $"{ID}";
        }
    }
}