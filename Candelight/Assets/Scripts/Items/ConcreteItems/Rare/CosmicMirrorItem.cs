using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class CosmicMirrorItem : AItem
    {
        protected override void ApplyProperty()
        {
            // Se lanza un proyectil cosmico extra detras del otro
            ARune.MageManager.AddExtraSpellThrow(1);
        }

        protected override void ResetProperty()
        {
            ARune.MageManager.AddExtraSpellThrow(-1);
        }
    }
}
