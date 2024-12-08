using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class OwlEyesItem : AItem
    {
        protected override void ApplyProperty()
        {
            GameSettings.Owl = true;
        }

        protected override void ResetProperty()
        {
            GameSettings.Owl = false;
        }
    }
}