using Controls;
using Hechizos;
using SpellInteractuable;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class CosmicClockItem : AItem
    {
        float _originalTimeScale;
        protected override void ApplyProperty()
        {
            // Al tener equipado el elemento cosmico, el tiempo se detiene completamente mientras se invocan hechizos o elementos en vez de tan solo ralentizarse. 
            _originalTimeScale = FindObjectOfType<UIManager>().GetSpellTimeScale();
            FindObjectOfType<UIManager>().SetSpellTimeScale(0.1f);
        }

        protected override void ResetProperty()
        {
            FindObjectOfType<UIManager>().SetSpellTimeScale(_originalTimeScale);
        }
    }
}