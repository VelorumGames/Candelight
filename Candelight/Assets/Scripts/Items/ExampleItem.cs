using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items 
{
    public class ExampleItem : AItem
    {
        private new void Awake()
        {
            base.Awake();
            Name = "Example";
        }
        protected override void ApplyProperty()
        {
            Debug.Log("Se ha activado el Item correctamente.");
        }
    }

}
