using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class MageInfo : ScriptableObject
    {
        public Dictionary<ESpellInstruction[], ARune> Spells = new Dictionary<ESpellInstruction[], ARune>();
    }
}
