using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Items.ConcreteItems
{
    public class WaxButterflyItem : AItem
    {
        protected override void ApplyProperty()
        {
            FindObjectOfType<PlayerController>().AddExtraLife(1);
        }

        protected override void ResetProperty()
        {
            FindObjectOfType<PlayerController>().AddExtraLife(-1);
        }
    }
}