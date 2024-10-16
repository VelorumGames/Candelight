using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "World Info")]
    public class WorldInfo : ScriptableObject
    {
        public int Seed;
        public GameObject World;
    }
}