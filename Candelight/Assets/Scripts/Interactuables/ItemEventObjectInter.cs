using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public class ItemEventObjectInter : AInteractuables
    {
        [SerializeField] GameObject _itemButton;

        public override void Interaction()
        {
            FindObjectOfType<Inventory>().AddItem(_itemButton, EItemCategory.Rare);
        }
    }
}