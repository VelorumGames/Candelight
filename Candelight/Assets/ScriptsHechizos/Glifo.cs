using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Glifo : MonoBehaviour
{
    public string Nombre { get; protected set; } // Nombre del glifo

    // Método abstracto para aplicar efectos a los glifos, que será implementado en las clases derivadas
    public abstract void AplicarEfecto();
 
}
