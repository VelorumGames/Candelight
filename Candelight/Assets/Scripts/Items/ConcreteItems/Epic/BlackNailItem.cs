using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class BlackNailItem : AItem
    {

        protected override void ApplyProperty()
        {
            if (ARune.FindSpell("Cosmic", out var spell) && spell is CosmicRune cSpell)
            {
                cSpell.AddPullFactor(0.1f);
                cSpell.AddPushFactor(0.05f);
            }
        }

        protected override void ResetProperty()
        {
            if (ARune.FindSpell("Cosmic", out var spell) && spell is CosmicRune cSpell)
            {
                cSpell.RemovePullFactor(0.1f);
                cSpell.RemovePushFactor(0.05f);
            }
        }
    }
}