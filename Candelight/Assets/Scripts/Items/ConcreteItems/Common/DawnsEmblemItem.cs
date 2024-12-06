using Hechizos.Elementales;
using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Items
{
    public class DawnsEmblemItem : AItem
    {
        float _ratio = 0.75f;

        protected override void ApplyProperty()
        {
            FindObjectOfType<PlayerController>().SetFastDelay(_ratio);
        }

        protected override void ResetProperty()
        {
            FindObjectOfType<PlayerController>().ResetFastDelay(_ratio);
        }
    }
}
