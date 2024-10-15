using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos;
using Hechizos.Elementales;

namespace Hechizos.DeForma
{
    public abstract class AShapeRune : ARune
    {
        public override void ApplyEffect()
        {

        }

        // Este método lanzará el hechizo basado en los elementos activos
        public void ThrowSorcery(AElementalRune[] currentElements)
        {
            Debug.Log("Lanzando hechizo: " + Name);
            // Aquí se define el efecto específico en función del elemento activo.
        }

    }
}
