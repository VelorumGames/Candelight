using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Map
{
    public abstract class ARoom : MonoBehaviour
    {
        protected int ID;
        public TextMeshPro _idText;

        public int GetID() => ID;
        public void SetID(int id)
        {
            ID = id;
            _idText.text = $"{ID}";
        }
    }
}