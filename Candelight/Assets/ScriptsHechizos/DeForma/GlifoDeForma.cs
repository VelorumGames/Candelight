using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlifoDeForma : Glifo
{
    protected string elementoActivo;// Almacena el elemento activo del mago

    public GlifoDeForma(string elementoActivo)
    {
        this.elementoActivo = elementoActivo;
    }

    // Este m�todo lanzar� el hechizo basado en el elemento activo
    public override void AplicarEfecto()
    {
        //Debug.Log("Lanzando hechizo: " + Nombre + " con elemento: " + elementoActivo);
        // Aqu� se define el efecto espec�fico en funci�n del elemento activo.
    }

}
