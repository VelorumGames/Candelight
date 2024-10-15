using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mago : MonoBehaviour
{
    private string elementoActivo = "Fuego"; // Propiedad que mantiene el elemento activo del mago

    // Método para cambiar el elemento activo cuando se usa un glifo elemental
    public void CambiarElementoActivo(GlifoElemental glifo)
    {
        elementoActivo = glifo.Nombre;
        Debug.Log("Elemento activo ahora es: " + elementoActivo);
    }

    // Método para obtener el elemento activo actual
    public string GetElementoActivo()
    {
        return elementoActivo;
    }

    // Método para lanzar un hechizo con un glifo de forma
    public void LanzarHechizo(GlifoDeForma glifoForma)
    {
        glifoForma.AplicarEfecto(); // Llama al método que aplica el efecto del glifo de forma
    }
}
