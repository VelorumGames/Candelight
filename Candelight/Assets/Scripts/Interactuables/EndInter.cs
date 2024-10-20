using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public class EndInter : AInteractuables
    {
        public override void Interaction()
        {
            Debug.Log("Se pasa a la siguiente zona");
            MapManager.Instance.EndLevel();
        }
    }
}