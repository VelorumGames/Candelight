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
        public ELevel[] LevelTypes;
        public EBiome Biome;
        public NodeManager Node;
        public int EventID;
        public EEventSolution EventSolution;
    }
}
