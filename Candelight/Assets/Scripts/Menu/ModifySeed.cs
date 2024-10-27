using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Menu
{
    public class ModifySeed : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        public void UpdateSeed(string str)
        {
            int seed = int.Parse(str);
            Random.InitState(seed);
            _world.Seed = seed;
        }
    }
}
