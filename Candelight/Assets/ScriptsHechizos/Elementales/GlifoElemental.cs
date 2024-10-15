using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlifoElemental : Glifo
{

    public override void AplicarEfecto()
    {
        // Aquí se establece la propiedad elemental en el personaje.
        Debug.Log("Propiedad elemental activada: " + Nombre);
    }

}
