using Hechizos;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class IdrianVialItem : AItem
    {
        protected override void ApplyProperty()
        {
            ARune.SetExtraElement(true);
        }

        protected override void ResetProperty()
        {
            ARune.SetExtraElement(false);
        }
    }
}