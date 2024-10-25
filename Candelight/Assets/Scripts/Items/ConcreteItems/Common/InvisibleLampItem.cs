using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Items.ConcreteItems
{
    public class InvisibleLampItem : AItem
    {

        public WorldInfo World;
        // La vela se agota un 5% menos cada vez que se pasa un nodo. 



        protected override void ApplyProperty()
        {
            World.NodeCandleFactor *= 0.95f;
        }

        protected override void ResetProperty()
        {
            World.NodeCandleFactor /= 0.95f;
        }

    }
}