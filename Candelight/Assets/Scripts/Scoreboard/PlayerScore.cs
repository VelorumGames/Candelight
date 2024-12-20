using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scoreboard
{
    [CreateAssetMenu(menuName = "Player Score")]
    public class PlayerScore : ScriptableObject
    {
        public string Name;
        public int CompletedNodes;
        public int NumObjects;
        public int CompletedEvents;
        public int EnemiesKilled;
    }
}