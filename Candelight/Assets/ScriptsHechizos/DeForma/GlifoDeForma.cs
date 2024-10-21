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

    // Este método lanzará el hechizo basado en el elemento activo
    public override void AplicarEfecto()
    {
        //Debug.Log("Lanzando hechizo: " + Nombre + " con elemento: " + elementoActivo);
        // Aquí se define el efecto específico en función del elemento activo.
    }

}
