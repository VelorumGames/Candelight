using Hechizos.DeForma;
using Hechizos;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellInteractuable;

namespace Items.ConcreteItems
{
    public class FireTraceItem : AItem
    {
        protected override void ApplyProperty()
        {
            // Los proyectiles de fuego dejan un rastro incendiario al lanzarse
            ARune.MageManager.ShowTrail(EElements.Fire);
        }

        protected override void ResetProperty()
        {
            ARune.MageManager.HideTrail(EElements.Fire);
        }
    }
}