using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class ElectricFingersItem : AItem
    {


        protected override void ApplyProperty()
        {
            FindObjectOfType<PlayerController>().RemoveCandleFactor(0.15f);
            GameSettings.ElectricFingers = true;
        }

        protected override void ResetProperty()
        {
            FindObjectOfType<PlayerController>().AddCandleFactor(0.15f);
            GameSettings.ElectricFingers = false;
        }
    }
}