using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public abstract class ASorceryInteractuable : MonoBehaviour
    {
        public abstract bool Interaction(AElementalRune element);
    }
}
