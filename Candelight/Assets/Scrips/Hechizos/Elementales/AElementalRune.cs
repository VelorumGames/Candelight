using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos.Elementales
{
    public abstract class AElementalRune : ARune
    {

        public override void ApplyEffect()
        {
            // Aqu� se establece la propiedad elemental en el personaje.
            Debug.Log("Propiedad elemental activada: " + Name);
        }

    }

}