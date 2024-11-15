using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class ArcaneTridentItem : AItem
    {

        protected override void ApplyProperty()
        {
            ARune.MageManager.SetExtraProjectiles(true);
        }

        protected override void ResetProperty()
        {
            ARune.MageManager.SetExtraProjectiles(false);
        }
    }
}
