using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class IronOfCourageItem : AItem
    {
        PlayerController _cont;

        protected override void ApplyProperty()
        {
            if (_cont == null) _cont = FindObjectOfType<PlayerController>();
            _cont.AddLastSpellTime(1.5f);
        }

        protected override void ResetProperty()
        {
            _cont.RemoveLastSpellTime(1.5f);
        }
    }
}