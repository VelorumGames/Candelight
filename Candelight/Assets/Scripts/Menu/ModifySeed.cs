using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Menu
{
    public class ModifySeed : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        int _onlineSeed;

        private void Awake()
        {
            if (!GameSettings.Online) gameObject.SetActive(true);
            else
            {
                StartCoroutine(ManageOnlineSeed());
            }
        }

        IEnumerator ManageOnlineSeed()
        {
            yield return Database.Get<OnlineSeed>("Seed", RecieveSeed);
            Random.InitState(_onlineSeed);
            _world.Seed = _onlineSeed;
        }

        public void UpdateSeed(string str)
        {
            int seed = int.Parse(str);
            Random.InitState(seed);
            _world.Seed = seed;
        }

        void RecieveSeed(OnlineSeed seedObj)
        {
            if (seedObj != null) _onlineSeed = seedObj.Seed;
        }
    }

    [System.Serializable]
    class OnlineSeed
    {
        public int Seed;
    }
}
