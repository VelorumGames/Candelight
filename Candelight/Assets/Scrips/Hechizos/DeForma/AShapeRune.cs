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

        // Este m�todo lanzar� el hechizo basado en los elementos activos
        public void ThrowSorcery(AElementalRune[] currentElements)
        {
            Debug.Log("Lanzando hechizo: " + Name);
            // Aqu� se define el efecto espec�fico en funci�n del elemento activo.
        }

    }
}
