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
            if (GameSettings.Online)
            {
                StartCoroutine(ManageOnlineSeed());
            }
        }

        IEnumerator ManageOnlineSeed()
        {
            Debug.Log("Buscando seed");
            yield return Database.Get<OnlineSeed>("Seed", RecieveSeed);
            Debug.Log("SEED RECIBIDA: " + _onlineSeed);
            Random.InitState(_onlineSeed);
            _world.Seed = _onlineSeed;
            GameSettings.Seed = _onlineSeed;

            gameObject.SetActive(false);
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
