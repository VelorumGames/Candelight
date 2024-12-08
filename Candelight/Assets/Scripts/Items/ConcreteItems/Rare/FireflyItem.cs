using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class FireflyItem : AItem
    {
        public GameObject Firefly;

        GameObject _firefly;

        protected override void ApplyProperty()
        {
            _firefly = Instantiate(Firefly);
            DontDestroyOnLoad(_firefly);
        }

        protected override void ResetProperty()
        {
            Destroy(_firefly);
        }
    }
}
