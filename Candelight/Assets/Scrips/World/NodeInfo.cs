using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Node Info")]
    public class NodeInfo : ScriptableObject
    {
        public string Name;
        public int Levels;
        public int CurrentLevel;
        public int[] Seeds;
        public EBiome Biome;
    }
}
